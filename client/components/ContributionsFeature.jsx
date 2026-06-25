'use client';

import React, { useState, useEffect } from 'react';
import { Star, Award, Check, Plus, Trophy, Heart, ThumbsUp, User, AlertCircle } from 'lucide-react';
import { useRole } from '../context/RoleContext';

export default function ContributionsFeature() {
  const { capabilities, graphqlFetch, token, activeRole } = useRole();
  const [activeTab, setActiveTab] = useState('feed'); // feed, catalog, leaderboard, approval

  const [contributions, setContributions] = useState([]);
  const [catalogItems, setCatalogItems] = useState([]);
  const [leaderboard, setLeaderboard] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  // Submit form state
  const [showSubmitModal, setShowSubmitModal] = useState(false);
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [contribType, setContribType] = useState('self-initiated');
  const [category, setCategory] = useState('innovation');
  const [suggestedPoints, setSuggestedPoints] = useState('50');
  const [impactLevel, setImpactLevel] = useState('medium');
  const [approvalForms, setApprovalForms] = useState({});

  const handleFormChange = (id, field, value) => {
    setApprovalForms(prev => ({
      ...prev,
      [id]: {
        ...prev[id],
        [field]: value
      }
    }));
  };

  const loadContributionsData = async () => {
    if (!token) return;
    setLoading(true);
    setError('');
    try {
      // 1. Fetch Contributions list
      const queryContrib = `
        query GetContributions {
          contributions {
            id
            title
            description
            contributionType
            category
            status
            points
            suggestedPoints
            impactLevel
            employeeName
            approverName
            approvalComments
            submittedDate
            approvedDate
          }
        }
      `;
      const contribData = await graphqlFetch(queryContrib);
      if (contribData && contribData.contributions) {
        setContributions(contribData.contributions);
      }

      // 2. Fetch Catalog Items
      const queryCatalog = `
        query GetAvailableContributionItems {
          availableContributionItems {
            id
            title
            description
            category
            suggestedPoints
            status
            claimedByEmployee
          }
        }
      `;
      const catalogData = await graphqlFetch(queryCatalog);
      if (catalogData && catalogData.availableContributionItems) {
        setCatalogItems(catalogData.availableContributionItems);
      }

      // 3. Fetch Leaderboard
      const queryLeaderboard = `
        query GetContributionLeaderboard {
          contributionLeaderboard {
            id
            employeeName
            department
            totalPoints
            badges
            averageRating
          }
        }
      `;
      const leaderboardData = await graphqlFetch(queryLeaderboard);
      if (leaderboardData && leaderboardData.contributionLeaderboard) {
        setLeaderboard(leaderboardData.contributionLeaderboard);
      }
    } catch (err) {
      setError(err.message || 'Error loading contributions data');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadContributionsData();
  }, [token, activeRole]);

  const handleClaim = async (itemId) => {
    setLoading(true);
    setError('');
    try {
      const mutation = `
        mutation ClaimContributionItem($command: ClaimContributionItemCommandInput!) {
          claimContributionItem(command: $command) {
            id
            status
            claimedByEmployee
          }
        }
      `;
      const variables = {
        command: {
          itemId
        }
      };
      await graphqlFetch(mutation, variables);
      await loadContributionsData();
    } catch (err) {
      setError(err.message || 'Error claiming catalog item');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmitContribution = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      const mutation = `
        mutation SubmitContribution($command: SubmitContributionCommandInput!) {
          submitContribution(command: $command) {
            id
            title
          }
        }
      `;
      const variables = {
        command: {
          title,
          description,
          contributionType: contribType,
          category,
          suggestedPoints: parseInt(suggestedPoints) || 50,
          impactLevel
        }
      };
      await graphqlFetch(mutation, variables);
      setTitle('');
      setDescription('');
      setShowSubmitModal(false);
      await loadContributionsData();
    } catch (err) {
      setError(err.message || 'Contribution submission failed');
    } finally {
      setLoading(false);
    }
  };

  const handleApprove = async (id, approve, finalPoints, comments) => {
    setLoading(true);
    setError('');
    try {
      const mutation = `
        mutation ApproveContribution($command: ApproveContributionCommandInput!) {
          approveContribution(command: $command) {
            id
            status
            points
          }
        }
      `;
      const variables = {
        command: {
          contributionId: id,
          finalPoints: approve ? (parseInt(finalPoints) || 50) : 0,
          comments: comments || (approve ? 'Approved' : 'Rejected')
        }
      };
      await graphqlFetch(mutation, variables);
      await loadContributionsData();
    } catch (err) {
      setError(err.message || 'Processing contribution approval failed');
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

  // Filter contributions under-review for the approval tab
  const approvalQueue = contributions.filter(c => c.status === 'under-review' || c.status === 'Submitted');

  return (
    <div className="animate-fade-in space-y-6">
      <div className="section-header flex justify-between items-center">
        <div>
          <h1 className="section-title"><Star className="w-7 h-7 text-teal-500" /> Value Contributions</h1>
          <p className="section-subtitle">Gamified value initiatives, contribution catalog, and company leaderboard</p>
        </div>
        <button className="btn-teal py-2.5 text-xs font-bold" onClick={() => setShowSubmitModal(true)}>
          + Submit Contribution
        </button>
      </div>

      {error && (
        <div className="p-4 bg-rose-500/10 border border-rose-500/20 text-rose-400 text-sm rounded-xl">
          {error}
        </div>
      )}

      {/* Tabs */}
      <div className="tab-bar">
        <button className={`tab-btn ${activeTab === 'feed' ? 'active' : ''}`} onClick={() => setActiveTab('feed')}>My Contributions ({contributions.length})</button>
        <button className={`tab-btn ${activeTab === 'catalog' ? 'active' : ''}`} onClick={() => setActiveTab('catalog')}>Contribution Catalog</button>
        <button className={`tab-btn ${activeTab === 'leaderboard' ? 'active' : ''}`} onClick={() => setActiveTab('leaderboard')}>Company Leaderboard</button>
        {capabilities.canApproveExpense && (
          <button className={`tab-btn ${activeTab === 'approval' ? 'active' : ''}`} onClick={() => setActiveTab('approval')}>
            Approval Queue ({approvalQueue.length})
          </button>
        )}
      </div>

      {loading && <div className="p-12 text-center text-slate-400">Syncing ledger records...</div>}

      {/* Tab 1: My Contributions Feed */}
      {!loading && activeTab === 'feed' && (
        contributions.length === 0 ? (
          <div className="glass-card text-center p-8 text-slate-500 italic">
            No value contribution ledger records found. Submit a new contribution above!
          </div>
        ) : (
          <div className="space-y-4">
            {contributions.map((cn) => (
              <div key={cn.id} className="glass-card flex flex-col md:flex-row md:items-center justify-between gap-4 border-l-4 border-l-teal-500">
                <div>
                  <div className="flex items-center gap-2 mb-1">
                    <h3 className="font-bold text-white text-base">{cn.title}</h3>
                    <span className="badge badge-teal uppercase text-[10px]">{cn.category}</span>
                    <span className="badge badge-slate uppercase text-[10px]">{cn.contributionType}</span>
                  </div>
                  <p className="text-sm text-slate-300 mb-2">{cn.description}</p>
                  <div className="flex items-center gap-3 text-xs text-slate-400">
                    <span>Impact: <strong className="text-amber-400 uppercase">{cn.impactLevel}</strong></span>
                    <span>Submitted By: <strong className="text-slate-200">{cn.employeeName}</strong></span>
                    {cn.approverName && <span>Approved By: <strong className="text-emerald-400">{cn.approverName}</strong></span>}
                  </div>
                  {cn.approvalComments && (
                    <div className="text-xs text-slate-400 italic mt-2 bg-slate-950/20 p-2 rounded-lg">
                      "{cn.approvalComments}"
                    </div>
                  )}
                </div>
                <div className="flex items-center gap-4 border-t md:border-t-0 border-white/10 pt-3 md:pt-0">
                  <div className="text-right">
                    <div className="text-xs text-slate-400">Awarded Points</div>
                    <div className="text-2xl font-extrabold text-teal-400">{cn.points} <span className="text-xs text-slate-500 font-normal">/ {cn.suggestedPoints}</span></div>
                  </div>
                  <div className="flex flex-col gap-2 items-end">
                    <span className={`badge ${
                      cn.status === 'approved' || cn.status === 'completed' 
                        ? 'badge-success' 
                        : cn.status === 'under-review' || cn.status === 'Submitted' 
                          ? 'badge-warning' 
                          : 'badge-danger'
                    }`}>
                      {cn.status.toUpperCase()}
                    </span>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )
      )}

      {/* Tab 2: Catalog */}
      {!loading && activeTab === 'catalog' && (
        catalogItems.length === 0 ? (
          <div className="glass-card text-center p-8 text-slate-500 italic">No available contribution catalog initiatives.</div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            {catalogItems.map((cat) => (
              <div key={cat.id} className="glass-card flex flex-col justify-between space-y-4">
                <div>
                  <div className="flex items-center justify-between mb-2">
                    <span className="badge badge-teal uppercase text-[10px]">{cat.category}</span>
                    <span className="font-extrabold text-teal-400 text-sm">{cat.suggestedPoints} Points</span>
                  </div>
                  <h3 className="font-bold text-white text-base mb-2">{cat.title}</h3>
                  <p className="text-xs text-slate-300">{cat.description}</p>
                </div>
                <div className="border-t border-white/10 pt-3">
                  {cat.claimedByEmployee ? (
                    <button className="w-full py-2 bg-slate-800 text-slate-400 font-bold text-xs rounded-xl border border-white/5 cursor-not-allowed" disabled>
                      ✓ Claimed by {cat.claimedByEmployee}
                    </button>
                  ) : (
                    <button className="btn-teal w-full py-2 justify-center text-xs" onClick={() => handleClaim(cat.id)}>
                      Claim Initiative
                    </button>
                  )}
                </div>
              </div>
            ))}
          </div>
        )
      )}

      {/* Tab 3: Leaderboard */}
      {!loading && activeTab === 'leaderboard' && (
        leaderboard.length === 0 ? (
          <div className="glass-card text-center p-8 text-slate-500 italic">No leaderboard records generated.</div>
        ) : (
          <div className="glass-card max-w-3xl mx-auto space-y-4">
            <div className="flex items-center justify-between border-b border-white/10 pb-4">
              <h2 className="text-lg font-bold text-slate-100 flex items-center gap-2">
                <Trophy className="w-5 h-5 text-amber-500" /> Value Leaderboard
              </h2>
              <span className="badge badge-slate">Updated Live</span>
            </div>
            <div className="space-y-3">
              {leaderboard.map((lb, index) => (
                <div key={lb.id} className="p-4 rounded-2xl bg-slate-900/60 border border-white/5 flex items-center justify-between">
                  <div className="flex items-center gap-4">
                    <div className={`w-10 h-10 rounded-2xl border flex items-center justify-center font-extrabold text-base ${
                      index === 0 ? 'text-amber-400 border-amber-500/40 bg-amber-500/10' : index === 1 ? 'text-slate-300 border-slate-400/40 bg-slate-400/10' : 'text-slate-500 border-white/10 bg-slate-900'
                    }`}>
                      #{index + 1}
                    </div>
                    <div>
                      <div className="flex items-center gap-2">
                        <h4 className="font-bold text-white text-base">{lb.employeeName}</h4>
                        <span className="badge badge-teal text-[10px]">{lb.badges || 'Contributor'}</span>
                      </div>
                      <div className="text-xs text-slate-400">Department: <strong className="text-slate-200">{lb.department}</strong></div>
                    </div>
                  </div>
                  <div className="text-right">
                    <div className="text-2xl font-extrabold text-teal-400">{lb.totalPoints}</div>
                    <div className="text-[10px] text-slate-500 uppercase font-semibold">Total Points</div>
                  </div>
                </div>
              ))}
            </div>
          </div>
        )
      )}

      {/* Tab 4: Approval Queue */}
      {!loading && activeTab === 'approval' && capabilities.canApproveExpense && (
        <div className="glass-card">
          <h2 className="text-lg font-bold text-slate-100 mb-4">Manager Contribution Approval Queue</h2>
          {approvalQueue.length === 0 ? (
            <div className="empty-state text-center p-6 text-slate-500 italic">
              No value contribution initiatives are waiting for review.
            </div>
          ) : (
            <div className="space-y-4">
              {approvalQueue.map((item) => {
                const itemState = approvalForms[item.id] || { points: item.suggestedPoints, comment: '' };
                return (
                <div key={item.id} className="p-5 rounded-2xl bg-slate-900/60 border border-white/10 flex flex-col md:flex-row md:items-center justify-between gap-4">
                  <div className="flex-grow">
                    <div className="flex items-center gap-2 mb-1">
                      <h4 className="font-bold text-white text-sm">Employee: {item.employeeName}</h4>
                      <span className="badge badge-teal uppercase text-[10px]">{item.category}</span>
                    </div>
                    <div className="text-sm font-semibold text-slate-200 mb-1">{item.title} (Suggested: <strong>{item.suggestedPoints} Pts</strong>)</div>
                    <p className="text-xs text-slate-400 bg-slate-800/80 p-2.5 rounded-xl border border-white/5">{item.description}</p>
                  </div>
                  <div className="flex items-center gap-2 self-end md:self-center">
                    <div className="flex items-center gap-1 bg-slate-800 px-2 py-1.5 rounded-xl border border-white/10">
                      <span className="text-xs text-slate-400">Award:</span>
                      <input 
                        type="number" 
                        className="w-16 bg-transparent text-white font-bold text-xs focus:outline-none text-right" 
                        value={itemState.points}
                        onChange={(e) => handleFormChange(item.id, 'points', e.target.value)}
                      />
                    </div>
                    <input 
                      type="text" 
                      placeholder="Add comments..." 
                      className="form-control max-w-[160px] text-xs py-2" 
                      value={itemState.comment}
                      onChange={(e) => handleFormChange(item.id, 'comment', e.target.value)}
                    />
                    <button className="btn-teal py-2 text-xs" onClick={() => {
                      handleApprove(item.id, true, itemState.points, itemState.comment);
                    }}>Approve</button>
                    <button className="btn-danger py-2 text-xs" onClick={() => {
                      handleApprove(item.id, false, 0, itemState.comment);
                    }}>Reject</button>
                  </div>
                </div>
                );
              })}
            </div>
          )}
        </div>
      )}

      {/* Submit Modal */}
      {showSubmitModal && (
        <div className="modal-backdrop">
          <div className="modal-panel">
            <div className="modal-header">
              <h3 className="text-lg font-bold text-slate-100">Submit Value Initiative</h3>
              <button onClick={() => setShowSubmitModal(false)} className="text-slate-400 hover:text-white">&times;</button>
            </div>
            <form onSubmit={handleSubmitContribution}>
              <div className="modal-body space-y-4">
                <div className="form-group">
                  <label className="form-label">Initiative Title</label>
                  <input type="text" className="form-control" value={title} onChange={(e) => setTitle(e.target.value)} required placeholder="e.g. Automated E2E Test Framework" />
                </div>
                <div className="form-group">
                  <label className="form-label">Details / Description</label>
                  <textarea className="form-control" rows="3" value={description} onChange={(e) => setDescription(e.target.value)} required placeholder="Detail the business value and impact of this initiative..." />
                </div>
                <div className="grid grid-cols-2 gap-4">
                  <div className="form-group">
                    <label className="form-label">Contribution Type</label>
                    <select className="form-control" value={contribType} onChange={(e) => setContribType(e.target.value)}>
                      <option value="self-initiated">Self-Initiated</option>
                      <option value="assigned">Assigned Task</option>
                      <option value="committed">Committed OKR</option>
                    </select>
                  </div>
                  <div className="form-group">
                    <label className="form-label">Category</label>
                    <select className="form-control" value={category} onChange={(e) => setCategory(e.target.value)}>
                      <option value="process-improvement">Process Improvement</option>
                      <option value="innovation">Innovation / Hackathon</option>
                      <option value="customer-satisfaction">Customer Satisfaction</option>
                      <option value="cost-saving">Cost Reduction</option>
                      <option value="team-building">Team Building</option>
                    </select>
                  </div>
                </div>
                <div className="grid grid-cols-2 gap-4">
                  <div className="form-group">
                    <label className="form-label">Suggested Points</label>
                    <input type="number" className="form-control" value={suggestedPoints} onChange={(e) => setSuggestedPoints(e.target.value)} required />
                  </div>
                  <div className="form-group">
                    <label className="form-label">Impact Level</label>
                    <select className="form-control" value={impactLevel} onChange={(e) => setImpactLevel(e.target.value)}>
                      <option value="low">Low Impact</option>
                      <option value="medium">Medium Impact</option>
                      <option value="high">High Business Impact</option>
                    </select>
                  </div>
                </div>
              </div>
              <div className="modal-footer">
                <button type="button" className="btn-ghost" onClick={() => setShowSubmitModal(false)}>Cancel</button>
                <button type="submit" className="btn-teal">Submit Ledger Entry</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
