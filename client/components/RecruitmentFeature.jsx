'use client';

import React, { useState } from 'react';
import { Briefcase, UserCheck, Check, Search, Calendar, UserPlus, FileText } from 'lucide-react';
import { useRole } from '../context/RoleContext';

export default function RecruitmentFeature() {
  const { activeRole, capabilities } = useRole();
  const [activeTab, setActiveTab] = useState('pipeline'); // postings, pipeline
  const [statusFilter, setStatusFilter] = useState('all');
  const [selectedCand, setSelectedCand] = useState(null);

  const jobPostings = [
    { id: 'JOB-001', title: 'Senior React Developer', department: 'Engineering', location: 'Bangalore, IN', employmentType: 'Full-time', experience: '5+ years', salaryRange: '₹25-35 LPA', status: 'active', applicants: 45, shortlisted: 8, interviewing: 3, requirements: ['React', 'TypeScript', 'Node.js', '5+ years experience'], responsibilities: ['Lead frontend development', 'Code reviews', 'Mentor junior devs'] },
    { id: 'JOB-002', title: 'Product Manager', department: 'Product', location: 'Remote', employmentType: 'Full-time', experience: '4-7 years', salaryRange: '₹20-28 LPA', status: 'active', applicants: 62, shortlisted: 12, interviewing: 5, requirements: ['Product strategy', 'Agile/Scrum', 'Data analysis'], responsibilities: ['Define product roadmap', 'Work with engineering', 'Stakeholder management'] },
    { id: 'JOB-003', title: 'Data Scientist', department: 'Analytics', location: 'Hyderabad, IN', employmentType: 'Full-time', experience: '3-5 years', salaryRange: '₹18-25 LPA', status: 'closed', applicants: 38, shortlisted: 6, interviewing: 0, requirements: ['Python', 'ML', 'TensorFlow'], responsibilities: ['Build ML models', 'Analyze data', 'Insights reporting'] },
  ];

  const [candidates, setCandidates] = useState([
    { id: 'CAND-001', name: 'Ravi Kumar', appliedRole: 'Senior React Developer', status: 'shortlisted', rating: 4.5, email: 'ravi.k@email.com', phone: '+91 98765 43210', skills: ['React', 'TypeScript', 'GraphQL', 'Node.js'], experience: '6 years', expectedSalary: '₹30 LPA', noticePeriod: '60 days', notes: 'Strong technical candidate. Good cultural fit.', schedDate: '' },
    { id: 'CAND-002', name: 'Priya Sharma', appliedRole: 'Product Manager', status: 'interview-scheduled', rating: 4.0, email: 'priya.s@email.com', phone: '+91 98765 12345', skills: ['Product Strategy', 'Agile', 'Figma', 'SQL'], experience: '5 years', expectedSalary: '₹25 LPA', noticePeriod: '30 days', notes: 'Great product thinking. Schedule technical round.', schedDate: '2026-06-28 11:00 AM' },
    { id: 'CAND-003', name: 'Amit Patel', appliedRole: 'Data Scientist', status: 'new', rating: 3.5, email: 'amit.p@email.com', phone: '+91 99876 54321', skills: ['Python', 'TensorFlow', 'SQL', 'R'], experience: '3 years', expectedSalary: '₹20 LPA', noticePeriod: '90 days', notes: '', schedDate: '' },
    { id: 'CAND-004', name: 'Sneha Rao', appliedRole: 'Senior React Developer', status: 'offer-extended', rating: 4.8, email: 'sneha.r@email.com', phone: '+91 99123 45678', skills: ['React', 'Vue.js', 'TypeScript', 'AWS'], experience: '7 years', expectedSalary: '₹33 LPA', noticePeriod: '30 days', notes: 'Excellent candidate. Offer sent.', schedDate: '' },
  ]);

  // PSD Section 3.1: Access restriction
  if (!capabilities.canViewRecruitment) {
    return (
      <div className="glass-card empty-state">
        <div className="empty-icon"><Briefcase className="w-8 h-8 text-red-500" /></div>
        <h3 className="empty-title">Access Restricted</h3>
        <p className="empty-desc">The Recruitment & Candidate Pipeline module is restricted to HR and Administrator roles per company RBAC policies.</p>
      </div>
    );
  }

  const handleUpdateStatus = (id, newStatus) => {
    setCandidates(prev => prev.map(c => c.id === id ? { ...c, status: newStatus } : c));
    setSelectedCand(prev => prev ? { ...prev, status: newStatus } : null);
  };

  const filteredCand = statusFilter === 'all' ? candidates : candidates.filter(c => c.status === statusFilter);

  return (
    <div className="animate-fade-in space-y-6">
      <div className="section-header">
        <div>
          <h1 className="section-title"><Briefcase className="w-7 h-7 text-teal-500" /> Recruitment Pipeline</h1>
          <p className="section-subtitle">Candidate lifecycle tracking, interview scheduling, and active job requisitions</p>
        </div>
      </div>

      {/* Tabs */}
      <div className="tab-bar">
        <button className={`tab-btn ${activeTab === 'pipeline' ? 'active' : ''}`} onClick={() => setActiveTab('pipeline')}>Candidate Pipeline</button>
        <button className={`tab-btn ${activeTab === 'postings' ? 'active' : ''}`} onClick={() => setActiveTab('postings')}>Active Job Postings</button>
      </div>

      {/* Tab 1: Candidate Pipeline */}
      {activeTab === 'pipeline' && (
        <div className="space-y-6">
          {/* Filter Bar */}
          <div className="flex flex-wrap gap-2 p-2 bg-slate-900/50 rounded-2xl border border-white/10">
            {['all', 'new', 'screening', 'shortlisted', 'interview-scheduled', 'offer-extended'].map(st => (
              <button
                key={st}
                className={`px-4 py-2 rounded-xl text-xs font-bold uppercase tracking-wider transition-colors ${
                  statusFilter === st ? 'bg-teal-600 text-white' : 'text-slate-400 hover:bg-white/5'
                }`}
                onClick={() => setStatusFilter(st)}
              >
                {st.replace('-', ' ')}
              </button>
            ))}
          </div>

          {/* Candidate List */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            {filteredCand.map((cand) => (
              <div
                key={cand.id}
                onClick={() => setSelectedCand(cand)}
                className="glass-card cursor-pointer hover:border-teal-500/40 transition-all space-y-4 flex flex-col justify-between"
              >
                <div>
                  <div className="flex items-center justify-between mb-3 border-b border-white/10 pb-3">
                    <div className="flex items-center gap-3">
                      <div className="avatar avatar-md bg-teal-600 text-white font-bold">
                        {cand.name.split(' ').map(n => n[0]).join('')}
                      </div>
                      <div>
                        <h3 className="font-bold text-white text-base">{cand.name}</h3>
                        <p className="text-xs text-teal-400 font-semibold">{cand.appliedRole}</p>
                      </div>
                    </div>
                    <span className={`badge ${
                      cand.status === 'offer-extended' ? 'badge-success' : cand.status === 'interview-scheduled' ? 'badge-teal' : 'badge-warning'
                    }`}>
                      {cand.status.toUpperCase()}
                    </span>
                  </div>
                  <div className="text-xs text-slate-400 space-y-1">
                    <div>Exp: <strong className="text-slate-200">{cand.experience}</strong> · Rating: <strong className="text-amber-400">{cand.rating} ★</strong></div>
                    <div>Notice Period: <strong className="text-slate-200">{cand.noticePeriod}</strong></div>
                  </div>
                </div>
                <div className="text-[11px] text-slate-500 pt-2 border-t border-white/5 flex justify-between items-center">
                  <span>{cand.email}</span>
                  <span className="text-teal-400 font-bold">Click to manage &rarr;</span>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* Tab 2: Job Postings */}
      {activeTab === 'postings' && (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {jobPostings.map((job) => (
            <div key={job.id} className="glass-card space-y-4 flex flex-col justify-between">
              <div>
                <div className="flex items-center justify-between mb-2">
                  <h3 className="font-bold text-white text-base">{job.title}</h3>
                  <span className={`badge ${job.status === 'active' ? 'badge-success' : 'badge-slate'}`}>{job.status.toUpperCase()}</span>
                </div>
                <div className="text-xs text-slate-400 space-y-1 mb-4">
                  <div>Department: <strong className="text-slate-200">{job.department}</strong></div>
                  <div>Location: <strong className="text-slate-200">{job.location}</strong> ({job.employmentType})</div>
                  <div>Salary Range: <strong className="text-teal-400">{job.salaryRange}</strong></div>
                </div>
                <div className="border-t border-white/10 pt-3 space-y-2">
                  <div className="text-xs font-bold text-slate-300 uppercase tracking-wider">Key Requirements</div>
                  <div className="flex flex-wrap gap-1">
                    {job.requirements.map((req, i) => <span key={i} className="px-2 py-0.5 bg-slate-900 rounded text-[11px] text-slate-300">{req}</span>)}
                  </div>
                </div>
              </div>
              <div className="border-t border-white/10 pt-3 flex justify-between text-xs text-slate-400 font-semibold">
                <span>{job.applicants} Applicants</span>
                <span>{job.shortlisted} Shortlisted</span>
                <span>{job.interviewing} Interviewing</span>
              </div>
            </div>
          ))}
        </div>
      )}

      {/* Candidate Details Modal */}
      {selectedCand && (
        <div className="modal-backdrop">
          <div className="modal-panel max-w-2xl">
            <div className="modal-header">
              <div>
                <h3 className="text-xl font-bold text-white">{selectedCand.name}</h3>
                <p className="text-xs text-teal-400 font-semibold">{selectedCand.appliedRole}</p>
              </div>
              <button onClick={() => setSelectedCand(null)} className="text-slate-400 hover:text-white">&times;</button>
            </div>
            <div className="modal-body space-y-6">
              <div className="grid grid-cols-2 md:grid-cols-4 gap-4 bg-slate-900 p-4 rounded-2xl border border-white/10 text-xs">
                <div><span className="text-slate-500 block">Experience</span><strong className="text-white text-sm">{selectedCand.experience}</strong></div>
                <div><span className="text-slate-500 block">Rating</span><strong className="text-amber-400 text-sm">{selectedCand.rating} ★</strong></div>
                <div><span className="text-slate-500 block">Expected Salary</span><strong className="text-white text-sm">{selectedCand.expectedSalary}</strong></div>
                <div><span className="text-slate-500 block">Notice Period</span><strong className="text-white text-sm">{selectedCand.noticePeriod}</strong></div>
              </div>

              <div>
                <h4 className="text-xs font-bold text-slate-400 uppercase tracking-wider mb-2">Skills & Competencies</h4>
                <div className="flex flex-wrap gap-1.5">
                  {selectedCand.skills.map((s, i) => <span key={i} className="px-3 py-1 bg-teal-500/10 border border-teal-500/20 text-teal-300 rounded-xl text-xs font-bold">{s}</span>)}
                </div>
              </div>

              <div>
                <h4 className="text-xs font-bold text-slate-400 uppercase tracking-wider mb-2">Interview & Schedule Management</h4>
                <div className="flex items-center gap-2">
                  <input type="text" className="form-control text-xs py-2.5" placeholder="e.g. 2026-06-28 11:00 AM" defaultValue={selectedCand.schedDate} />
                  <button className="btn-teal py-2.5 text-xs flex-shrink-0"><Calendar className="w-3.5 h-3.5" /> Schedule Interview</button>
                </div>
              </div>

              <div>
                <h4 className="text-xs font-bold text-slate-400 uppercase tracking-wider mb-2">Update Pipeline Status</h4>
                <select
                  className="form-control text-xs py-2.5"
                  value={selectedCand.status}
                  onChange={(e) => handleUpdateStatus(selectedCand.id, e.target.value)}
                >
                  <option value="new" className="bg-slate-900">New Applicant</option>
                  <option value="screening" className="bg-slate-900">Screening</option>
                  <option value="shortlisted" className="bg-slate-900">Shortlisted</option>
                  <option value="interview-scheduled" className="bg-slate-900">Interview Scheduled</option>
                  <option value="interviewed" className="bg-slate-900">Interviewed</option>
                  <option value="offer-extended" className="bg-slate-900">Offer Extended</option>
                  <option value="hired" className="bg-slate-900">Hired</option>
                  <option value="rejected" className="bg-slate-900">Rejected</option>
                </select>
              </div>

              <div>
                <h4 className="text-xs font-bold text-slate-400 uppercase tracking-wider mb-2">HR Review Notes</h4>
                <textarea className="form-control text-xs" rows="3" defaultValue={selectedCand.notes}></textarea>
              </div>
            </div>
            <div className="modal-footer">
              <button className="btn-ghost" onClick={() => setSelectedCand(null)}>Close Panel</button>
              <button className="btn-teal" onClick={() => setSelectedCand(null)}>Save Changes</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
