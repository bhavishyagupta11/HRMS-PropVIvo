'use client';

import React, { useState, useEffect } from 'react';
import { Calendar, Check, X, Shield, CheckCircle2, AlertCircle } from 'lucide-react';
import { useRole } from '../context/RoleContext';

export default function LeaveFeature() {
  const { capabilities, activeRole, graphqlFetch, token } = useRole();
  const [activeTab, setActiveTab] = useState('balances'); // balances, requests, apply, approval

  // Leave Data States
  const [leaveBalances, setLeaveBalances] = useState([]);
  const [leaveRequests, setLeaveRequests] = useState([]);
  const [approvalQueue, setApprovalQueue] = useState([]);
  const [dataLoading, setDataLoading] = useState(false);
  const [dataError, setDataError] = useState(null);

  // New Leave Form State
  const [newLeaveType, setNewLeaveType] = useState('Casual Leave');
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [leaveReason, setLeaveReason] = useState('');
  const [formSubmitted, setFormSubmitted] = useState(false);
  const [formError, setFormError] = useState('');
  const [formLoading, setFormLoading] = useState(false);

  // Approval state
  const [approvalComments, setApprovalComments] = useState({});

  // Expandable row details for timeline
  const [expandedRequestId, setExpandedRequestId] = useState(null);

  // Helper mapping to C# role naming convention
  const getBackendRoleName = (role) => {
    if (role === 'Reporting Manager') return 'ReportingManager';
    return role;
  };

  // Fetch balances and requests when token is set
  useEffect(() => {
    if (token) {
      loadAllData();
    }
  }, [token]);

  // Reload data if activeRole changes (useful for approval queues role synchronization)
  useEffect(() => {
    if (token) {
      loadAllData();
    }
  }, [activeRole]);

  const loadAllData = async () => {
    setDataLoading(true);
    setDataError(null);
    try {
      await Promise.all([
        fetchBalances(),
        fetchRequests(),
        capabilities.canApproveLeave ? fetchPendingRequests() : Promise.resolve(),
      ]);
    } catch (err) {
      setDataError(err.message || 'Error fetching data from server.');
    } finally {
      setDataLoading(false);
    }
  };

  const fetchBalances = async () => {
    const query = `
      query GetMyLeaveBalances {
        myLeaveBalances {
          id
          leaveType
          totalAllowed
          used
          pending
          available
          carriedForward
          encashed
        }
      }
    `;
    const data = await graphqlFetch(query);
    if (data && data.myLeaveBalances) {
      setLeaveBalances(data.myLeaveBalances);
      // Auto select first leave type if available
      if (data.myLeaveBalances.length > 0) {
        setNewLeaveType(data.myLeaveBalances[0].leaveType);
      }
    }
  };

  const fetchRequests = async () => {
    const query = `
      query GetMyLeaveRequests {
        myLeaveRequests {
          id
          leaveType
          startDate
          endDate
          totalDays
          reason
          status
          currentApprovalLevel
          approvals {
            id
            level
            status
            approverId
            comment
            actionedAt
          }
        }
      }
    `;
    const data = await graphqlFetch(query);
    if (data && data.myLeaveRequests) {
      setLeaveRequests(data.myLeaveRequests);
    }
  };

  const fetchPendingRequests = async () => {
    const query = `
      query GetPendingLeaveRequests {
        pendingLeaveRequests {
          id
          userId
          employeeName
          leaveType
          startDate
          endDate
          totalDays
          reason
          status
          currentApprovalLevel
          approvals {
            id
            level
            status
            approverId
            comment
            actionedAt
          }
        }
      }
    `;
    const data = await graphqlFetch(query);
    if (data && data.pendingLeaveRequests) {
      setApprovalQueue(data.pendingLeaveRequests);
    }
  };

  // Auto calculate days
  const calculateDays = () => {
    if (!startDate || !endDate) return 0;
    const start = new Date(startDate);
    const end = new Date(endDate);
    const diffTime = Math.abs(end - start);
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24)) + 1;
    return diffDays > 0 ? diffDays : 0;
  };

  const handleApplyLeave = async (e) => {
    e.preventDefault();
    setFormError('');
    setFormLoading(true);

    const totalDays = calculateDays();
    if (totalDays <= 0) {
      setFormError('Invalid date range.');
      setFormLoading(false);
      return;
    }

    // Validate balance locally first
    const balance = leaveBalances.find(b => b.leaveType === newLeaveType);
    if (balance && balance.available < totalDays) {
      setFormError(`Insufficient leave balance. You only have ${balance.available} days available.`);
      setFormLoading(false);
      return;
    }

    try {
      const query = `
        mutation SubmitLeaveRequest($input: SubmitLeaveRequestDtoInput!) {
          submitLeaveRequest(input: $input) {
            id
            status
          }
        }
      `;
      const variables = {
        input: {
          leaveType: newLeaveType,
          startDate: new Date(startDate).toISOString(),
          endDate: new Date(endDate).toISOString(),
          totalDays,
          reason: leaveReason
        }
      };

      await graphqlFetch(query, variables);
      setFormSubmitted(true);
      
      // Reload lists
      await loadAllData();

      setTimeout(() => {
        setFormSubmitted(false);
        setStartDate('');
        setEndDate('');
        setLeaveReason('');
        setActiveTab('requests');
      }, 1500);
    } catch (err) {
      setFormError(err.message || 'Error submitting leave request.');
    } finally {
      setFormLoading(false);
    }
  };

  const handleProcessApproval = async (requestId, approve, comments) => {
    try {
      const query = `
        mutation ProcessLeaveRequest($input: ProcessLeaveRequestDtoInput!) {
          processLeaveRequest(input: $input) {
            id
            status
          }
        }
      `;
      const variables = {
        input: {
          requestId,
          approve,
          comments
        }
      };

      await graphqlFetch(query, variables);
      await loadAllData();
    } catch (err) {
      alert(`Approval processing failed: ${err.message}`);
    }
  };

  const handleCancelRequest = async (requestId) => {
    if (!confirm('Are you sure you want to cancel this leave request?')) return;
    try {
      const query = `
        mutation CancelLeaveRequest($requestId: String!) {
          cancelLeaveRequest(requestId: $requestId) {
            id
            status
          }
        }
      `;
      await graphqlFetch(query, { requestId });
      await loadAllData();
    } catch (err) {
      alert(`Cancellation failed: ${err.message}`);
    }
  };

  const selectedBalance = leaveBalances.find(b => b.leaveType === newLeaveType) || leaveBalances[0];

  // Helper to format date strings cleanly
  const formatDate = (isoString) => {
    if (!isoString) return '--';
    return new Date(isoString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  };

  // Helper to get styling for timelines
  const getApprovalStepState = (approval, request) => {
    // If request rejected at/before this, show rejected if this is the active/actioned one
    if (approval.status === 'Rejected') return 'bg-rose-500 border-rose-600 text-white';
    if (approval.status === 'Approved') return 'bg-emerald-500 border-emerald-600 text-white';
    if (approval.status === 'Cancelled') return 'bg-slate-700 border-slate-800 text-slate-400';

    if (request.status === 'Rejected' || request.status === 'Cancelled') {
      return 'bg-slate-800 border-slate-900 text-slate-500';
    }

    if (request.currentApprovalLevel === approval.level) {
      return 'bg-amber-500 border-amber-600 text-white animate-pulse';
    }

    return 'bg-slate-800 border-slate-700 text-slate-500';
  };

  return (
    <div className="animate-fade-in space-y-6">
      <div className="section-header flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <h1 className="section-title flex items-center gap-2">
            <Calendar className="w-7 h-7 text-teal-500" /> Leave Management
          </h1>
          <p className="section-subtitle">Database-integrated workflow, multi-level approvals, and balance logs</p>
        </div>
        <div className="flex items-center gap-3">
          <span className="text-xs text-slate-400 flex items-center gap-1.5">
            <Shield className="w-3.5 h-3.5 text-emerald-400" /> Role: {activeRole}
          </span>
        </div>
      </div>

      {/* Tabs */}
      <div className="tab-bar">
        <button className={`tab-btn ${activeTab === 'balances' ? 'active' : ''}`} onClick={() => setActiveTab('balances')}>
          My Balances
        </button>
        <button className={`tab-btn ${activeTab === 'requests' ? 'active' : ''}`} onClick={() => setActiveTab('requests')}>
          My Requests
        </button>
        <button className={`tab-btn ${activeTab === 'apply' ? 'active' : ''}`} onClick={() => setActiveTab('apply')}>
          Apply for Leave
        </button>
        {capabilities.canApproveLeave && (
          <button className={`tab-btn ${activeTab === 'approval' ? 'active' : ''}`} onClick={() => setActiveTab('approval')}>
            Approval Queue ({approvalQueue.length})
          </button>
        )}
      </div>

      {dataError && (
        <div className="p-4 bg-rose-500/10 border border-rose-500/20 text-rose-400 rounded-xl flex items-center justify-between">
          <span className="text-sm font-medium">{dataError}</span>
          <button onClick={loadAllData} className="text-xs px-3 py-1 rounded-lg bg-rose-500/20 hover:bg-rose-500/30 font-bold transition">
            Retry Connection
          </button>
        </div>
      )}

      {dataLoading || !token ? (
        <div className="p-12 text-center text-slate-400 space-y-2">
          <div className="w-6 h-6 border-2 border-teal-500 border-t-transparent rounded-full animate-spin mx-auto"></div>
          <p className="text-xs font-semibold">Synchronizing with Postgres database...</p>
        </div>
      ) : (
        <>
          {/* Tab 1: Balances */}
          {activeTab === 'balances' && (
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 animate-fade-in">
              {leaveBalances.map((bal) => (
                <div key={bal.id} className="glass-card flex flex-col justify-between p-5 border border-white/5 rounded-2xl bg-slate-900/40">
                  <div>
                    <div className="flex items-center justify-between mb-2">
                      <h3 className="font-bold text-white text-sm">{bal.leaveType}</h3>
                      <span className="badge badge-slate">Total {bal.totalAllowed}</span>
                    </div>
                    <div className="text-4xl font-extrabold my-3 text-teal-400">{bal.available}</div>
                    <div className="text-xs text-slate-400 mb-4">Available Days</div>
                  </div>
                  <div className="border-t border-white/10 pt-3 space-y-1.5 text-xs text-slate-300">
                    <div className="flex justify-between"><span>Used:</span> <span className="font-semibold text-white">{bal.used} days</span></div>
                    <div className="flex justify-between"><span>Pending Approval:</span> <span className="font-semibold text-amber-400">{bal.pending} days</span></div>
                    {bal.carriedForward > 0 && <div className="flex justify-between text-teal-400"><span>Carried Forward:</span> <span>+{bal.carriedForward}</span></div>}
                    {bal.encashed > 0 && <div className="flex justify-between text-amber-400"><span>Encashed:</span> <span>{bal.encashed} days</span></div>}
                  </div>
                </div>
              ))}
            </div>
          )}

          {/* Tab 2: My Requests History */}
          {activeTab === 'requests' && (
            <div className="glass-card animate-fade-in">
              <h2 className="text-lg font-bold text-slate-100 mb-4">My Leave History</h2>
              {leaveRequests.length === 0 ? (
                <div className="p-8 text-center text-slate-500 text-sm">
                  You have not submitted any leave requests yet.
                </div>
              ) : (
                <div className="overflow-x-auto">
                  <table className="data-table">
                    <thead>
                      <tr>
                        <th>Leave Type</th>
                        <th>Dates</th>
                        <th>Days</th>
                        <th>Reason</th>
                        <th>Status</th>
                        <th>Routing Flow</th>
                        <th className="text-right">Actions</th>
                      </tr>
                    </thead>
                    <tbody>
                      {leaveRequests.map((req) => (
                        <React.Fragment key={req.id}>
                          <tr className="hover:bg-white/5 transition">
                            <td className="font-bold text-teal-400">{req.leaveType}</td>
                            <td className="text-xs">{formatDate(req.startDate)} to {formatDate(req.endDate)}</td>
                            <td className="font-semibold text-white">{req.totalDays} Days</td>
                            <td className="max-w-xs truncate text-xs text-slate-400">{req.reason}</td>
                            <td>
                              <span className={`badge ${
                                req.status === 'Approved' ? 'badge-success' : req.status === 'Pending' ? 'badge-warning' : 'badge-danger'
                              }`}>
                                {req.status}
                              </span>
                            </td>
                            <td>
                              <button
                                onClick={() => setExpandedRequestId(expandedRequestId === req.id ? null : req.id)}
                                className="text-xs text-teal-400 hover:text-teal-300 font-semibold underline"
                              >
                                View Level Progress
                              </button>
                            </td>
                            <td className="text-right">
                              {req.status === 'Pending' && (
                                <button
                                  onClick={() => handleCancelRequest(req.id)}
                                  className="px-2.5 py-1 text-xs font-bold text-rose-400 hover:bg-rose-500/10 border border-rose-500/20 rounded-lg transition"
                                >
                                  Cancel Request
                                </button>
                              )}
                            </td>
                          </tr>
                          {expandedRequestId === req.id && (
                            <tr>
                              <td colSpan="7" className="bg-slate-950/40 p-4 border-t border-white/5 animate-slide-down">
                                <div className="space-y-4 max-w-2xl">
                                  <h4 className="text-xs font-bold text-slate-400 uppercase tracking-wider">Multi-Level Approval Timeline</h4>
                                  
                                  <div className="relative flex items-center justify-between py-2 pl-4">
                                    <div className="absolute left-[36px] right-[36px] top-1/2 h-0.5 bg-slate-800 -translate-y-1/2 -z-10"></div>
                                    
                                    {req.approvals.map((app) => (
                                      <div key={app.id} className="flex flex-col items-center space-y-1.5 relative z-10">
                                        <div className={`w-8 h-8 rounded-full border-2 flex items-center justify-center font-bold text-xs ${getApprovalStepState(app, req)}`}>
                                          {app.status === 'Approved' ? <Check className="w-4 h-4" /> : app.status === 'Rejected' ? <X className="w-4 h-4" /> : '?'}
                                        </div>
                                        <span className="text-[11px] font-semibold text-slate-300">
                                          {app.level === 'ReportingManager' ? 'Reporting Manager' : app.level}
                                        </span>
                                        <span className="text-[9px] text-slate-400 capitalize">{app.status}</span>
                                      </div>
                                    ))}
                                  </div>

                                  <div className="space-y-2 mt-4 pt-3 border-t border-white/5">
                                    <h5 className="text-[10px] font-bold text-slate-500 uppercase tracking-wide">Approval Stage History Comments</h5>
                                    {req.approvals.some(a => a.comment) ? (
                                      <div className="space-y-2">
                                        {req.approvals.filter(a => a.comment).map((app) => (
                                          <div key={app.id} className="p-2.5 rounded-xl bg-slate-900 border border-white/5 text-xs">
                                            <div className="flex items-center justify-between text-slate-400 text-[10px] mb-1">
                                              <strong>{app.level === 'ReportingManager' ? 'Reporting Manager' : app.level}</strong>
                                              <span>{app.actionedAt ? new Date(app.actionedAt).toLocaleString() : ''}</span>
                                            </div>
                                            <p className="text-slate-300 italic">"{app.comment}"</p>
                                          </div>
                                        ))}
                                      </div>
                                    ) : (
                                      <p className="text-xs text-slate-500 italic">No comments recorded yet.</p>
                                    )}
                                  </div>
                                </div>
                              </td>
                            </tr>
                          )}
                        </React.Fragment>
                      ))}
                    </tbody>
                  </table>
                </div>
              )}
            </div>
          )}

          {/* Tab 3: Submit New Request */}
          {activeTab === 'apply' && (
            <div className="glass-card max-w-2xl mx-auto animate-fade-in">
              <h2 className="text-lg font-bold text-slate-100 mb-6">Submit New Leave Request</h2>
              {formSubmitted ? (
                <div className="p-6 bg-emerald-500/20 border border-emerald-500/30 rounded-xl text-center text-emerald-300 font-bold animate-fade-in">
                  Leave request submitted successfully! Sequential routing initialized...
                </div>
              ) : (
                <form onSubmit={handleApplyLeave} className="space-y-6">
                  {formError && (
                    <div className="p-3.5 bg-rose-500/10 border border-rose-500/20 text-rose-400 text-sm rounded-xl flex items-center gap-2">
                      <AlertCircle className="w-5 h-5 flex-shrink-0" />
                      <span>{formError}</span>
                    </div>
                  )}

                  <div className="form-group">
                    <label className="form-label">Leave Type</label>
                    <select
                      className="form-control"
                      value={newLeaveType}
                      onChange={(e) => setNewLeaveType(e.target.value)}
                    >
                      {leaveBalances.map(b => (
                        <option key={b.id} value={b.leaveType} className="bg-slate-900">
                          {b.leaveType} (Available: {b.available} days)
                        </option>
                      ))}
                    </select>
                    {selectedBalance && (
                      <div className="text-xs text-teal-400 mt-1.5 flex items-center gap-1">
                        <span>Available balance: <strong>{selectedBalance.available} Days</strong></span>
                      </div>
                    )}
                  </div>

                  <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <div className="form-group mb-0">
                      <label className="form-label">Start Date</label>
                      <input
                        type="date"
                        className="form-control"
                        value={startDate}
                        onChange={(e) => setStartDate(e.target.value)}
                        required
                      />
                    </div>
                    <div className="form-group mb-0">
                      <label className="form-label">End Date</label>
                      <input
                        type="date"
                        className="form-control"
                        value={endDate}
                        min={startDate}
                        onChange={(e) => setEndDate(e.target.value)}
                        required
                      />
                    </div>
                  </div>

                  <div className="form-group">
                    <label className="form-label">Total Days (Calculated)</label>
                    <input
                      type="text"
                      className="form-control bg-slate-800 text-teal-400 font-bold"
                      value={`${calculateDays()} Days`}
                      disabled
                    />
                  </div>

                  <div className="form-group">
                    <label className="form-label">Reason for Leave</label>
                    <textarea
                      className="form-control"
                      rows="3"
                      placeholder="Please details for the sequential routing approvals..."
                      value={leaveReason}
                      onChange={(e) => setLeaveReason(e.target.value)}
                      required
                    ></textarea>
                  </div>

                  <button type="submit" disabled={formLoading} className="btn-teal w-full py-3.5 justify-center text-base">
                    {formLoading ? 'Submitting to database...' : 'Submit Leave Request'}
                  </button>
                </form>
              )}
            </div>
          )}

          {/* Tab 4: Approval Queue */}
          {activeTab === 'approval' && capabilities.canApproveLeave && (
            <div className="glass-card animate-fade-in">
              <h2 className="text-lg font-bold text-slate-100 mb-4">
                Sequential Leave Approvals Queue ({activeRole})
              </h2>
              {approvalQueue.length === 0 ? (
                <div className="p-8 text-center text-slate-500 text-sm space-y-2">
                  <CheckCircle2 className="w-8 h-8 text-emerald-400 mx-auto" />
                  <p>All clean! There are no requests waiting for your role's approval at this level.</p>
                </div>
              ) : (
                <div className="space-y-4">
                  {approvalQueue.map((item) => (
                    <div key={item.id} className="p-5 rounded-2xl bg-slate-900/60 border border-white/10 flex flex-col gap-4">
                      <div className="flex flex-col md:flex-row md:items-center justify-between gap-2 border-b border-white/5 pb-3">
                        <div>
                          <div className="flex items-center gap-2 mb-1">
                            <h4 className="font-bold text-white text-base">Employee: {item.employeeName}</h4>
                            <span className="badge badge-teal">{item.leaveType}</span>
                          </div>
                          <div className="text-xs text-slate-400">
                            {formatDate(item.startDate)} to {formatDate(item.endDate)} (<strong>{item.totalDays} Days</strong>)
                          </div>
                        </div>
                        <div className="text-xs text-slate-300">
                          Active Stage: <strong className="text-amber-400 capitalize">{item.currentApprovalLevel}</strong>
                        </div>
                      </div>

                      <p className="text-sm text-slate-300 bg-slate-800/80 p-2.5 rounded-xl border border-white/5 italic">
                        "{item.reason}"
                      </p>

                      <div className="flex flex-col md:flex-row items-center gap-3">
                        <input
                          type="text"
                          placeholder="Provide approval/rejection comment..."
                          className="form-control text-xs flex-grow"
                          value={approvalComments[item.id] || ''}
                          onChange={(e) => setApprovalComments(prev => ({ ...prev, [item.id]: e.target.value }))}
                        />
                        <div className="flex items-center gap-2 w-full md:w-auto">
                          <button
                            className="btn-teal py-2.5 px-4 text-xs font-bold w-full md:w-auto justify-center"
                            onClick={() => {
                              const inputComment = approvalComments[item.id] || '';
                              handleProcessApproval(item.id, true, inputComment);
                            }}
                          >
                            Approve Stage
                          </button>
                          <button
                            className="btn-danger py-2.5 px-4 text-xs font-bold w-full md:w-auto justify-center"
                            onClick={() => {
                              const inputComment = approvalComments[item.id] || '';
                              handleProcessApproval(item.id, false, inputComment);
                            }}
                          >
                            Reject & Stop Flow
                          </button>
                        </div>
                      </div>
                    </div>
                  ))}
                </div>
              )}
            </div>
          )}
        </>
      )}
    </div>
  );
}
