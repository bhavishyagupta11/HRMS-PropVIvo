'use client';

import React, { useState, useEffect } from 'react';
import { FileText, Upload, AlertTriangle, CheckCircle2, XCircle, Clock, Download } from 'lucide-react';
import { useRole } from '../context/RoleContext';

const REQUIRED_DOCS = [
  { name: 'Passport', category: 'identity' },
  { name: 'Offer Letter', category: 'employment' },
  { name: 'Work Authorization (H1-B)', category: 'work-auth' },
  { name: 'PAN Card', category: 'tax' },
  { name: 'Degree Certificate', category: 'education' },
  { name: 'Aadhar Card', category: 'identity' },
];

export default function DocumentFeature() {
  const { capabilities, graphqlFetch, token, activeRole } = useRole();
  const [selectedCategory, setSelectedCategory] = useState('all');

  const [dbDocuments, setDbDocuments] = useState([]);
  const [pendingQueue, setPendingQueue] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const loadDocsData = async () => {
    if (!token) return;
    setLoading(true);
    setError('');
    try {
      // 1. Fetch My Documents
      const myDocsQuery = `
        query GetMyDocuments {
          myDocuments {
            id
            userId
            category
            fileName
            fileType
            fileSize
            blobUrl
            uploadDate
            expiryDate
            verificationStatus
            rejectionReason
            verificationDate
          }
        }
      `;
      const myDocsData = await graphqlFetch(myDocsQuery);
      if (myDocsData && myDocsData.myDocuments) {
        setDbDocuments(myDocsData.myDocuments);
      }

      // 2. Fetch Pending Verification Queue if HR/Admin
      const isApprover = activeRole === 'HR' || activeRole === 'Admin' || capabilities.canApproveLeave;
      if (isApprover) {
        const pendingQuery = `
          query GetPendingDocuments {
            pendingDocuments {
              id
              userId
              category
              fileName
              fileType
              fileSize
              blobUrl
              uploadDate
              expiryDate
              verificationStatus
              rejectionReason
              verificationDate
            }
          }
        `;
        const pendingData = await graphqlFetch(pendingQuery);
        if (pendingData && pendingData.pendingDocuments) {
          setPendingQueue(pendingData.pendingDocuments);
        }
      }
    } catch (err) {
      setError(err.message || 'Error loading document repository');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadDocsData();
  }, [token, activeRole]);

  const handleFileUpload = (e, category, name) => {
    const file = e.target.files[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = async () => {
      const base64 = reader.result.split(',')[1];
      setLoading(true);
      setError('');
      try {
        const mutation = `
          mutation UploadDocument($input: UploadDocumentDtoInput!) {
            uploadDocument(input: $input) {
              id
              fileName
              verificationStatus
            }
          }
        `;
        const variables = {
          input: {
            category,
            fileName: name,
            fileType: file.type || 'application/pdf',
            fileSize: file.size,
            base64Content: base64,
            expiryDate: category === 'identity' || category === 'work-auth' ? new Date(Date.now() + 365*24*60*60*1000*5).toISOString() : null
          }
        };
        await graphqlFetch(mutation, variables);
        await loadDocsData();
      } catch (err) {
        setError(err.message || 'File upload failed');
      } finally {
        setLoading(false);
      }
    };
    reader.readAsDataURL(file);
  };

  const handleVerify = async (id, verify, reason = '') => {
    setLoading(true);
    setError('');
    try {
      const mutation = `
        mutation VerifyDocument($input: VerifyDocumentDtoInput!) {
          verifyDocument(input: $input) {
            id
            verificationStatus
          }
        }
      `;
      const variables = {
        input: {
          documentId: id,
          isApproved: verify,
          rejectionReason: verify ? null : (reason || 'Fails compliance checks')
        }
      };
      await graphqlFetch(mutation, variables);
      await loadDocsData();
    } catch (err) {
      setError(err.message || 'Verification update failed');
    } finally {
      setLoading(false);
    }
  };

  // Build local list of documents by joining REQUIRED_DOCS with dbDocuments
  const buildDocumentsList = () => {
    return REQUIRED_DOCS.map(req => {
      // Find matching doc uploaded in this category with this exact file/doc name
      const match = dbDocuments.find(d => d.category === req.category && d.fileName === req.name);
      if (match) {
        return {
          id: match.id,
          name: match.fileName,
          category: match.category,
          status: match.verificationStatus.toLowerCase(),
          uploadDate: match.uploadDate ? new Date(match.uploadDate).toISOString().split('T')[0] : null,
          verifiedDate: match.verificationDate ? new Date(match.verificationDate).toISOString().split('T')[0] : null,
          expiryDate: match.expiryDate ? new Date(match.expiryDate).toISOString().split('T')[0] : null,
          fileSize: `${(match.fileSize / (1024 * 1024)).toFixed(1)} MB`,
          fileType: match.fileType.includes('pdf') ? 'PDF' : 'Image',
          blobUrl: match.blobUrl,
          rejectionReason: match.rejectionReason
        };
      }
      return {
        id: `REQ-${req.category}-${req.name}`,
        name: req.name,
        category: req.category,
        status: 'missing',
        uploadDate: null,
        verifiedDate: null,
        expiryDate: null,
        fileSize: null,
        fileType: null,
        blobUrl: null
      };
    });
  };

  const allDocs = buildDocumentsList();
  
  // Also include any other custom documents uploaded that are not in the REQUIRED_DOCS list
  const customDocs = dbDocuments
    .filter(d => !REQUIRED_DOCS.some(req => req.category === d.category && req.name === d.fileName))
    .map(match => ({
      id: match.id,
      name: match.fileName,
      category: match.category,
      status: match.verificationStatus.toLowerCase(),
      uploadDate: match.uploadDate ? new Date(match.uploadDate).toISOString().split('T')[0] : null,
      verifiedDate: match.verificationDate ? new Date(match.verificationDate).toISOString().split('T')[0] : null,
      expiryDate: match.expiryDate ? new Date(match.expiryDate).toISOString().split('T')[0] : null,
      fileSize: `${(match.fileSize / (1024 * 1024)).toFixed(1)} MB`,
      fileType: match.fileType.includes('pdf') ? 'PDF' : 'Image',
      blobUrl: match.blobUrl,
      rejectionReason: match.rejectionReason
    }));

  const combinedDocs = [...allDocs, ...customDocs];

  const categories = [
    { id: 'all', label: 'All Documents' },
    { id: 'identity', label: 'Identity' },
    { id: 'employment', label: 'Employment' },
    { id: 'work-auth', label: 'Work Auth' },
    { id: 'tax', label: 'Tax Documents' },
    { id: 'education', label: 'Education' },
  ];

  const isHR = activeRole === 'HR' || activeRole === 'Admin' || capabilities.canApproveLeave;
  if (isHR) {
    categories.push({ id: 'verification-queue', label: `Pending Queue (${pendingQueue.length})` });
  }

  const filteredDocs = selectedCategory === 'all' 
    ? combinedDocs 
    : selectedCategory === 'verification-queue'
      ? pendingQueue.map(p => ({
          id: p.id,
          name: p.fileName,
          category: p.category,
          status: p.verificationStatus.toLowerCase(),
          uploadDate: p.uploadDate ? new Date(p.uploadDate).toISOString().split('T')[0] : null,
          verifiedDate: p.verificationDate ? new Date(p.verificationDate).toISOString().split('T')[0] : null,
          expiryDate: p.expiryDate ? new Date(p.expiryDate).toISOString().split('T')[0] : null,
          fileSize: `${(p.fileSize / (1024 * 1024)).toFixed(1)} MB`,
          fileType: p.fileType.includes('pdf') ? 'PDF' : 'Image',
          blobUrl: p.blobUrl,
          rejectionReason: p.rejectionReason,
          employeeId: p.userId
        }))
      : combinedDocs.filter(d => d.category === selectedCategory);

  return (
    <div className="animate-fade-in space-y-6">
      <div className="section-header">
        <div>
          <h1 className="section-title"><FileText className="w-7 h-7 text-teal-500" /> Document Repository</h1>
          <p className="section-subtitle">Centralized document vault with verification workflows and expiry tracking</p>
        </div>
      </div>

      {error && (
        <div className="p-4 bg-rose-500/10 border border-rose-500/20 text-rose-400 text-sm rounded-xl">
          {error}
        </div>
      )}

      {/* Filter Tabs */}
      <div className="tab-bar">
        {categories.map((cat) => (
          <button
            key={cat.id}
            className={`tab-btn ${selectedCategory === cat.id ? 'active' : ''}`}
            onClick={() => setSelectedCategory(cat.id)}
          >
            {cat.label}
          </button>
        ))}
      </div>

      {loading && <div className="p-12 text-center text-slate-400">Syncing repository...</div>}

      {/* Document Grid */}
      {!loading && (
        filteredDocs.length === 0 ? (
          <div className="glass-card empty-state">
            <div className="empty-icon"><FileText className="w-8 h-8 text-slate-500" /></div>
            <h3 className="empty-title">No Documents Found</h3>
            <p className="empty-desc">No documents match the active view criteria.</p>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {filteredDocs.map((doc) => (
              <div key={doc.id} className="glass-card flex flex-col justify-between space-y-4">
                <div>
                  <div className="flex items-center justify-between mb-2">
                    <h3 className="font-bold text-white text-base truncate max-w-[180px]" title={doc.name}>
                      {doc.name}
                    </h3>
                    <span className="badge badge-slate uppercase text-[10px]">{doc.category}</span>
                  </div>

                  {doc.employeeId && (
                    <div className="text-xs text-teal-400 font-bold mb-2">
                      Owner: {doc.employeeId}
                    </div>
                  )}

                  {/* Status Badge */}
                  <div className="mb-4">
                    <span className={`badge ${
                      doc.status === 'verified' ? 'badge-success' : doc.status === 'uploaded' || doc.status === 'pending' ? 'badge-teal' : 'badge-danger'
                    }`}>
                      {doc.status === 'verified' && <CheckCircle2 className="w-3 h-3 mr-1" />}
                      {(doc.status === 'uploaded' || doc.status === 'pending') && <Clock className="w-3 h-3 mr-1" />}
                      {(doc.status === 'rejected' || doc.status === 'missing') && <XCircle className="w-3 h-3 mr-1" />}
                      {doc.status.toUpperCase()}
                    </span>
                  </div>

                  {/* Details */}
                  <div className="space-y-1.5 text-xs text-slate-400">
                    {doc.uploadDate && <div>Uploaded: <strong className="text-slate-300">{doc.uploadDate}</strong> ({doc.fileSize} · {doc.fileType})</div>}
                    {doc.verifiedDate && <div className="text-emerald-400">Verified on: <strong>{doc.verifiedDate}</strong></div>}
                    {doc.expiryDate && (
                      <div className="flex items-center gap-1 text-amber-400">
                        <AlertTriangle className="w-3.5 h-3.5" /> Expiry: <strong>{doc.expiryDate}</strong>
                      </div>
                    )}
                    {doc.rejectionReason && (
                      <div className="p-2.5 rounded-xl bg-red-500/10 border border-red-500/20 text-red-300 mt-2">
                        <strong>Rejection Reason:</strong> {doc.rejectionReason}
                      </div>
                    )}
                  </div>
                </div>

                {/* Actions */}
                <div className="border-t border-white/10 pt-4 flex flex-col gap-2">
                  {selectedCategory === 'verification-queue' ? (
                    <div className="space-y-2">
                      {doc.blobUrl && (
                        <a
                          href={doc.blobUrl}
                          target="_blank"
                          rel="noopener noreferrer"
                          className="btn-ghost w-full py-2 justify-center text-xs flex items-center gap-1.5"
                        >
                          <Download className="w-4 h-4" /> Download File
                        </a>
                      )}
                      <div className="grid grid-cols-2 gap-2 pt-2 border-t border-white/5">
                        <button className="btn-teal py-1.5 text-xs justify-center" onClick={() => handleVerify(doc.id, true)}>
                          Verify
                        </button>
                        <button className="btn-danger py-1.5 text-xs justify-center" onClick={() => {
                          const reason = prompt('Enter rejection reason:') || 'Document fails compliance verification';
                          handleVerify(doc.id, false, reason);
                        }}>
                          Reject
                        </button>
                      </div>
                    </div>
                  ) : (
                    <>
                      {doc.blobUrl && (
                        <a
                          href={doc.blobUrl}
                          target="_blank"
                          rel="noopener noreferrer"
                          className="btn-ghost w-full py-2 justify-center text-xs flex items-center gap-1.5 mb-1"
                        >
                          <Download className="w-4 h-4" /> Download / View
                        </a>
                      )}

                      <input
                        type="file"
                        id={`file-input-${doc.id}`}
                        style={{ display: 'none' }}
                        onChange={(e) => handleFileUpload(e, doc.category, doc.name)}
                      />
                      <label
                        htmlFor={`file-input-${doc.id}`}
                        className="btn-teal w-full py-2 justify-center text-xs flex items-center gap-1.5 cursor-pointer"
                      >
                        <Upload className="w-4 h-4" />
                        {doc.status === 'missing' ? 'Upload Document' : 'Replace File'}
                      </label>
                    </>
                  )}
                </div>
              </div>
            ))}
          </div>
        )
      )}
    </div>
  );
}
