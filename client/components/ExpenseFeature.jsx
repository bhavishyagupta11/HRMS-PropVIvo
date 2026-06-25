'use client';

import React, { useState, useEffect } from 'react';
import { Receipt, Upload, Check, AlertCircle, HelpCircle, DollarSign, Car } from 'lucide-react';
import { useRole } from '../context/RoleContext';

export default function ExpenseFeature() {
  const { capabilities, activeRole, graphqlFetch, token } = useRole();
  const [activeTab, setActiveTab] = useState('claims'); // claims, new, approval

  const [expenses, setExpenses] = useState([]);
  const [approvalQueue, setApprovalQueue] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [formSubmitted, setFormSubmitted] = useState(false);

  // Form state
  const [category, setCategory] = useState('travel');
  const [amount, setAmount] = useState('');
  const [currency, setCurrency] = useState('INR');
  const [description, setDescription] = useState('');
  const [claimDate, setClaimDate] = useState('');
  // Mileage state
  const [fromLoc, setFromLoc] = useState('');
  const [toLoc, setToLoc] = useState('');
  const [distance, setDistance] = useState('');
  const [vehicle, setVehicle] = useState('Car (₹12/km)');

  // Receipt state
  const [receiptFile, setReceiptFile] = useState(null);
  const [receiptFileName, setReceiptFileName] = useState(null);
  const [receiptFileType, setReceiptFileType] = useState(null);
  const [receiptBase64Content, setReceiptBase64Content] = useState(null);

  // Approval state
  const [approvalComments, setApprovalComments] = useState({});

  const loadExpensesData = async () => {
    if (!token) return;
    setLoading(true);
    setError('');
    try {
      // 1. Get my expenses
      const myExpQuery = `
        query GetMyExpenses {
          myExpenses {
            id
            category
            amount
            currency
            description
            expenseDate
            receiptUrl
            mileage
            isTaxable
            status
            approvalComments
            paidDate
          }
        }
      `;
      const myExpData = await graphqlFetch(myExpQuery);
      if (myExpData && myExpData.myExpenses) {
        setExpenses(myExpData.myExpenses);
      }

      // 2. Get pending approvals if authorized
      if (capabilities.canApproveExpense) {
        const pendingQuery = `
          query GetPendingExpenses {
            pendingExpenses {
              id
              userId
              employeeName
              category
              amount
              currency
              description
              expenseDate
              receiptUrl
              mileage
              isTaxable
              status
              approvalComments
            }
          }
        `;
        const pendingData = await graphqlFetch(pendingQuery);
        if (pendingData && pendingData.pendingExpenses) {
          setApprovalQueue(pendingData.pendingExpenses);
        }
      }
    } catch (err) {
      setError(err.message || 'Error loading expense data');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadExpensesData();
  }, [token, activeRole]);

  const handleCreateExpense = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    const vehicleRate = vehicle.includes('12') ? 12 : 6;
    const numericAmount = parseFloat(amount) || (category === 'travel' && distance ? parseFloat(distance) * vehicleRate : 0);
    if (!numericAmount) {
      setError('Please provide a valid expense amount or mileage distance.');
      setLoading(false);
      return;
    }

    try {
      const mutation = `
        mutation SubmitExpense($input: SubmitExpenseDtoInput!) {
          submitExpense(input: $input) {
            id
            status
          }
        }
      `;
      const variables = {
        input: {
          category,
          amount: numericAmount,
          currency,
          description: category === 'travel' && distance ? `Mileage (${distance}km from ${fromLoc} to ${toLoc} via ${vehicle})` : description,
          expenseDate: new Date(claimDate || new Date().toISOString().split('T')[0]).toISOString(),
          isTaxable: numericAmount > 5000,
          mileage: category === 'travel' && distance ? parseFloat(distance) : null,
          receiptFileName: receiptFileName,
          receiptFileType: receiptFileType,
          receiptBase64Content: receiptBase64Content
        }
      };

      const data = await graphqlFetch(mutation, variables);
      if (data && data.submitExpense) {
        setFormSubmitted(true);
        setAmount('');
        setDescription('');
        setDistance('');
        setFromLoc('');
        setToLoc('');
        setReceiptFile(null);
        setReceiptFileName(null);
        setReceiptFileType(null);
        setReceiptBase64Content(null);
        await loadExpensesData();
        setTimeout(() => {
          setFormSubmitted(false);
          setActiveTab('claims');
        }, 1500);
      }
    } catch (err) {
      setError(err.message || 'Expense submission failed');
    } finally {
      setLoading(false);
    }
  };

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setReceiptFile(file);
      setReceiptFileName(file.name);
      setReceiptFileType(file.type);
      const reader = new FileReader();
      reader.onloadend = () => {
        const base64String = reader.result.split(',')[1];
        setReceiptBase64Content(base64String);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleProcessApproval = async (expenseId, approve, comment) => {
    setLoading(true);
    setError('');
    try {
      const mutation = `
        mutation ProcessExpense($input: ProcessExpenseDtoInput!) {
          processExpense(input: $input) {
            id
            status
          }
        }
      `;
      const variables = {
        input: {
          expenseId,
          status: approve ? 'approved' : 'rejected',
          approvalComments: comment
        }
      };

      const data = await graphqlFetch(mutation, variables);
      if (data && data.processExpense) {
        await loadExpensesData();
      }
    } catch (err) {
      setError(err.message || 'Processing expense failed');
    } finally {
      setLoading(false);
    }
  };

  const formatDate = (dateStr) => {
    if (!dateStr) return '--';
    try {
      const d = new Date(dateStr);
      return d.toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' });
    } catch {
      return dateStr;
    }
  };

  return (
    <div className="animate-fade-in space-y-6">
      <div className="section-header">
        <div>
          <h1 className="section-title"><Receipt className="w-7 h-7 text-teal-500" /> Expense Claims</h1>
          <p className="section-subtitle">Submit expense receipts, calculate mileage, and track reimbursements</p>
        </div>
      </div>

      {error && (
        <div className="p-4 bg-rose-500/10 border border-rose-500/20 text-rose-400 text-sm rounded-xl">
          {error}
        </div>
      )}

      {/* Tabs */}
      <div className="tab-bar">
        <button className={`tab-btn ${activeTab === 'claims' ? 'active' : ''}`} onClick={() => setActiveTab('claims')}>
          My Claims
        </button>
        <button className={`tab-btn ${activeTab === 'new' ? 'active' : ''}`} onClick={() => setActiveTab('new')}>
          File New Claim
        </button>
        {capabilities.canApproveExpense && (
          <button className={`tab-btn ${activeTab === 'approval' ? 'active' : ''}`} onClick={() => setActiveTab('approval')}>
            Approval Queue ({approvalQueue.length})
          </button>
        )}
      </div>

      {/* Tab 1: Claims */}
      {activeTab === 'claims' && (
        <div className="glass-card">
          <h2 className="text-lg font-bold text-slate-100 mb-4">Reimbursement Records</h2>
          <div className="overflow-x-auto">
            <table className="data-table">
              <thead>
                <tr>
                  <th>Category</th>
                  <th>Amount</th>
                  <th>Date</th>
                  <th>Description</th>
                  <th>Taxable</th>
                  <th>Status</th>
                  <th>Approver Notes</th>
                </tr>
              </thead>
              <tbody>
                {expenses.length === 0 ? (
                  <tr>
                    <td colSpan="7" className="text-center p-6 text-slate-500">No expense claims found.</td>
                  </tr>
                ) : (
                  expenses.map((exp) => (
                    <tr key={exp.id}>
                      <td className="font-bold text-teal-400 capitalize">{exp.category}</td>
                      <td className="font-semibold text-white">{exp.currency} {exp.amount}</td>
                      <td>{formatDate(exp.expenseDate)}</td>
                      <td className="max-w-xs truncate text-xs text-slate-400">{exp.description}</td>
                      <td>{exp.isTaxable ? 'Yes' : 'No'}</td>
                      <td>
                        <span className={`badge ${
                          exp.status === 'approved' || exp.status === 'paid' ? 'badge-success' : exp.status === 'pending-approval' ? 'badge-warning' : 'badge-danger'
                        }`}>
                          {exp.status}
                        </span>
                      </td>
                      <td className="text-xs text-slate-400 italic">
                        {exp.approvalComments || 'None'}
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </div>
      )}

      {/* Tab 2: New Claim */}
      {activeTab === 'new' && (
        <div className="glass-card max-w-xl mx-auto">
          <h2 className="text-lg font-bold text-slate-100 mb-6">File New Expense Reimbursement</h2>
          {formSubmitted ? (
            <div className="p-6 bg-emerald-500/20 border border-emerald-500/30 rounded-xl text-center text-emerald-300 font-bold">
              Expense claim submitted successfully!
            </div>
          ) : (
            <form onSubmit={handleCreateExpense} className="space-y-6">
              <div className="form-group">
                <label className="form-label">Expense Category</label>
                <select className="form-control" value={category} onChange={(e) => setCategory(e.target.value)}>
                  <option value="travel">Travel & Transport</option>
                  <option value="food">Meals & Food</option>
                  <option value="accommodation">Accommodation</option>
                  <option value="communication">Communication</option>
                  <option value="medical">Medical Expenses</option>
                  <option value="office-supplies">Office Supplies</option>
                  <option value="other">Other Expenses</option>
                </select>
              </div>

              {category === 'travel' ? (
                <div className="p-4 bg-slate-950/40 rounded-2xl border border-white/5 space-y-4">
                  <div className="flex items-center gap-2 text-teal-400 font-semibold text-sm">
                    <Car className="w-4 h-4" /> Mileage Claim Mode
                  </div>
                  <div className="grid grid-cols-2 gap-4">
                    <div className="form-group mb-0">
                      <label className="form-label text-xs">From Location</label>
                      <input type="text" className="form-control text-xs" placeholder="e.g. HQ Office" value={fromLoc} onChange={(e) => setFromLoc(e.target.value)} />
                    </div>
                    <div className="form-group mb-0">
                      <label className="form-label text-xs">To Location</label>
                      <input type="text" className="form-control text-xs" placeholder="e.g. Client Site" value={toLoc} onChange={(e) => setToLoc(e.target.value)} />
                    </div>
                  </div>
                  <div className="grid grid-cols-2 gap-4">
                    <div className="form-group mb-0">
                      <label className="form-label text-xs">Distance (in km)</label>
                      <input type="number" className="form-control text-xs" placeholder="e.g. 24" value={distance} onChange={(e) => setDistance(e.target.value)} />
                    </div>
                    <div className="form-group mb-0">
                      <label className="form-label text-xs">Vehicle Type</label>
                      <select className="form-control text-xs" value={vehicle} onChange={(e) => setVehicle(e.target.value)}>
                        <option>Car (₹12/km)</option>
                        <option>Motorcycle (₹6/km)</option>
                      </select>
                    </div>
                  </div>
                </div>
              ) : (
                <div className="grid grid-cols-3 gap-4">
                  <div className="form-group col-span-1">
                    <label className="form-label">Currency</label>
                    <select className="form-control" value={currency} onChange={(e) => setCurrency(e.target.value)}>
                      <option value="INR">INR (₹)</option>
                      <option value="USD">USD ($)</option>
                      <option value="EUR">EUR (€)</option>
                    </select>
                  </div>
                  <div className="form-group col-span-2">
                    <label className="form-label">Amount</label>
                    <input type="number" className="form-control" placeholder="0.00" value={amount} onChange={(e) => setAmount(e.target.value)} required />
                  </div>
                </div>
              )}

              <div className="grid grid-cols-2 gap-4">
                <div className="form-group">
                  <label className="form-label">Date of Expense</label>
                  <input type="date" className="form-control" value={claimDate} onChange={(e) => setClaimDate(e.target.value)} required />
                </div>
                <div className="form-group">
                  <label className="form-label">Receipt Upload</label>
                  <label className="form-control flex items-center justify-between cursor-pointer border-dashed border-white/20 hover:border-teal-500/40 relative">
                    <input type="file" accept="image/*,.pdf" className="absolute inset-0 opacity-0 cursor-pointer" onChange={handleFileChange} />
                    <span className="text-xs text-slate-400 flex items-center gap-1.5"><Upload className="w-3.5 h-3.5" /> {receiptFileName || 'Select File...'}</span>
                  </label>
                </div>
              </div>

              <div className="form-group">
                <label className="form-label">Purpose / Description</label>
                <textarea className="form-control" rows="3" placeholder="Provide description..." value={description} onChange={(e) => setDescription(e.target.value)} required={category !== 'travel'}></textarea>
              </div>

              <button type="submit" disabled={loading} className="btn-teal w-full py-3.5 justify-center">
                {loading ? 'Submitting...' : 'Submit Claim'}
              </button>
            </form>
          )}
        </div>
      )}

      {/* Tab 3: Approval Queue */}
      {activeTab === 'approval' && capabilities.canApproveExpense && (
        <div className="glass-card">
          <h2 className="text-lg font-bold text-slate-100 mb-4">Pending Team Expense Claims</h2>
          {approvalQueue.length === 0 ? (
            <div className="text-center p-6 text-slate-500 italic">No claims waiting for review.</div>
          ) : (
            <div className="space-y-4">
              {approvalQueue.map((item) => (
                <div key={item.id} className="p-4 rounded-xl border border-white/5 bg-slate-900/40 flex flex-col gap-3">
                  <div className="flex items-center justify-between">
                    <div>
                      <h4 className="font-bold text-white text-sm">Employee: {item.employeeName}</h4>
                      <div className="text-xs text-slate-400 capitalize">{item.category} • {formatDate(item.expenseDate)}</div>
                    </div>
                    <div className="text-right">
                      <div className="font-bold text-teal-400">{item.currency} {item.amount}</div>
                    </div>
                  </div>
                  <p className="text-xs text-slate-300 bg-slate-950/20 p-2.5 rounded-lg">"{item.description}"</p>
                  <div className="flex items-center gap-3">
                    <input type="text" placeholder="Add optional comments..." className="form-control text-xs flex-grow" value={approvalComments[item.id] || ''} onChange={(e) => setApprovalComments(prev => ({ ...prev, [item.id]: e.target.value }))} />
                    <button className="btn-teal py-2 text-xs" onClick={() => {
                      const comment = approvalComments[item.id] || '';
                      handleProcessApproval(item.id, true, comment);
                    }}>Approve</button>
                    <button className="btn-danger py-2 text-xs" onClick={() => {
                      const comment = approvalComments[item.id] || '';
                      handleProcessApproval(item.id, false, comment);
                    }}>Reject</button>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      )}
    </div>
  );
}
