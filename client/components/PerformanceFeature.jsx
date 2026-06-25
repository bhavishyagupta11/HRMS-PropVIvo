'use client';

import React, { useState, useEffect } from 'react';
import { Target, Star, Award, TrendingUp, Calendar, AlertCircle } from 'lucide-react';
import { useRole } from '../context/RoleContext';

export default function PerformanceFeature() {
  const { capabilities, graphqlFetch, token, activeRole } = useRole();
  const [activeTab, setActiveTab] = useState('goals'); // goals, review

  const [goals, setGoals] = useState([]);
  const [reviews, setReviews] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  // Goal Form State
  const [showGoalModal, setShowGoalModal] = useState(false);
  const [goalTitle, setGoalTitle] = useState('');
  const [goalDesc, setGoalDesc] = useState('');
  const [goalTargetDate, setGoalTargetDate] = useState('');
  const [goalMetricTarget, setGoalMetricTarget] = useState('');

  // Review Form State
  const [showReviewModal, setShowReviewModal] = useState(false);
  const [reviewCycle, setReviewCycle] = useState('Q2 2026');
  const [selfRating, setSelfRating] = useState('4.0');
  const [managerRating, setManagerRating] = useState('4.0');
  const [reviewComments, setReviewComments] = useState('');
  const [reviewerId, setReviewerId] = useState('');

  const loadPerformanceData = async () => {
    if (!token) return;
    setLoading(true);
    setError('');
    try {
      // 1. Fetch Goals
      const goalsQuery = `
        query GetMyGoals {
          myGoals {
            id
            userId
            title
            description
            targetDate
            status
            metricTarget
            metricActual
          }
        }
      `;
      const goalsData = await graphqlFetch(goalsQuery);
      if (goalsData && goalsData.myGoals) {
        setGoals(goalsData.myGoals);
      }

      // 2. Fetch Reviews
      const reviewsQuery = `
        query GetMyReviews {
          myReviews {
            id
            userId
            reviewCycle
            selfRating
            managerRating
            comments
            reviewerUserId
          }
        }
      `;
      const reviewsData = await graphqlFetch(reviewsQuery);
      if (reviewsData && reviewsData.myReviews) {
        setReviews(reviewsData.myReviews);
      }
    } catch (err) {
      setError(err.message || 'Error loading performance data');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadPerformanceData();
  }, [token, activeRole]);

  const handleCreateGoal = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      const mutation = `
        mutation CreateGoal($command: CreateGoalCommandInput!) {
          createGoal(command: $command) {
            id
            title
          }
        }
      `;
      const variables = {
        command: {
          title: goalTitle,
          description: goalDesc,
          targetDate: new Date(goalTargetDate || new Date().toISOString()).toISOString(),
          metricTarget: parseFloat(goalMetricTarget) || 100
        }
      };
      await graphqlFetch(mutation, variables);
      setGoalTitle('');
      setGoalDesc('');
      setGoalTargetDate('');
      setGoalMetricTarget('');
      setShowGoalModal(false);
      await loadPerformanceData();
    } catch (err) {
      setError(err.message || 'Goal creation failed');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmitReview = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      const mutation = `
        mutation SubmitReview($command: SubmitReviewCommandInput!) {
          submitReview(command: $command) {
            id
            reviewCycle
          }
        }
      `;
      const variables = {
        command: {
          reviewCycle,
          selfRating: parseFloat(selfRating),
          managerRating: parseFloat(managerRating),
          comments: reviewComments,
          reviewerUserId: reviewerId
        }
      };
      await graphqlFetch(mutation, variables);
      setReviewComments('');
      setShowReviewModal(false);
      await loadPerformanceData();
    } catch (err) {
      setError(err.message || 'Review submission failed');
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

  // Compute weighted goal progress for overall dashboard
  const overallProgress = goals.length > 0
    ? Math.round(
        goals.reduce((acc, g) => {
          const comp = g.metricTarget > 0 ? (g.metricActual / g.metricTarget) * 100 : 0;
          return acc + Math.min(100, Math.max(0, comp));
        }, 0) / goals.length
      )
    : 0;

  return (
    <div className="animate-fade-in space-y-6">
      <div className="section-header flex justify-between items-center">
        <div>
          <h1 className="section-title"><Target className="w-7 h-7 text-teal-500" /> Performance & Goals</h1>
          <p className="section-subtitle">Goal/OKR management with key results tracking and periodic performance reviews</p>
        </div>
        <div className="flex gap-2">
          {activeTab === 'goals' && (
            <button className="btn-teal py-2.5 text-xs font-bold" onClick={() => setShowGoalModal(true)}>
              + Add Goal / OKR
            </button>
          )}
          {activeTab === 'review' && (
            <button className="btn-teal py-2.5 text-xs font-bold" onClick={() => setShowReviewModal(true)}>
              + File Performance Review
            </button>
          )}
        </div>
      </div>

      {error && (
        <div className="p-4 bg-rose-500/10 border border-rose-500/20 text-rose-400 text-sm rounded-xl">
          {error}
        </div>
      )}

      {/* Tabs */}
      <div className="tab-bar">
        <button className={`tab-btn ${activeTab === 'goals' ? 'active' : ''}`} onClick={() => setActiveTab('goals')}>
          Goals & OKRs
        </button>
        <button className={`tab-btn ${activeTab === 'review' ? 'active' : ''}`} onClick={() => setActiveTab('review')}>
          Performance Review ({reviews.length})
        </button>
      </div>

      {loading && <div className="p-12 text-center text-slate-400">Loading performance records...</div>}

      {/* Tab 1: Goals & OKRs */}
      {!loading && activeTab === 'goals' && (
        <div className="space-y-6">
          {/* Overall Progress Bar */}
          <div className="glass-card">
            <div className="flex items-center justify-between mb-2">
              <h3 className="font-bold text-white text-base">Overall OKR Progress</h3>
              <span className="font-extrabold text-teal-400 text-lg">{overallProgress}%</span>
            </div>
            <div className="progress-bar h-3 bg-slate-900">
              <div className="progress-fill progress-teal" style={{ width: `${overallProgress}%` }}></div>
            </div>
            <p className="text-xs text-slate-400 mt-2">Based on database tracking of {goals.length} active goals</p>
          </div>

          {/* Goal Cards */}
          {goals.length === 0 ? (
            <div className="glass-card text-center p-8 text-slate-500 italic">
              No active goals or OKRs found in database. Create a new goal above!
            </div>
          ) : (
            <div className="space-y-6">
              {goals.map((goal) => {
                const completionPct = goal.metricTarget > 0 
                  ? Math.round((goal.metricActual / goal.metricTarget) * 100) 
                  : 0;

                return (
                  <div key={goal.id} className="glass-card space-y-6">
                    <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 border-b border-white/10 pb-4">
                      <div>
                        <div className="flex items-center gap-2 mb-1">
                          <h3 className="text-lg font-bold text-white">{goal.title}</h3>
                          <span className="badge badge-teal uppercase text-[10px]">{goal.status}</span>
                        </div>
                        <p className="text-sm text-slate-400">{goal.description}</p>
                      </div>
                      <div className="flex items-center gap-4">
                        <div className="text-right">
                          <div className="text-xs text-slate-400">Target Date: <strong className="text-slate-300">{formatDate(goal.targetDate)}</strong></div>
                          <div className="text-xs text-slate-400">Target Metric: <strong className="text-teal-400">{goal.metricTarget}</strong></div>
                        </div>
                      </div>
                    </div>

                    {/* Goal Progress */}
                    <div>
                      <div className="flex justify-between text-xs font-semibold text-slate-300 mb-1.5">
                        <span>Goal Completion Progress</span>
                        <span>{completionPct}% ({goal.metricActual} / {goal.metricTarget})</span>
                      </div>
                      <div className="progress-bar bg-slate-900">
                        <div 
                          className={`progress-fill ${completionPct > 70 ? 'progress-teal' : completionPct > 40 ? 'progress-warning' : 'bg-gradient-to-r from-orange-600 to-orange-400'}`} 
                          style={{ width: `${Math.min(100, completionPct)}%` }}
                        ></div>
                      </div>
                    </div>
                  </div>
                );
              })}
            </div>
          )}
        </div>
      )}

      {/* Tab 2: Performance Review */}
      {!loading && activeTab === 'review' && (
        reviews.length === 0 ? (
          <div className="glass-card text-center p-8 text-slate-500 italic">
            No performance review records found in database. File a self-evaluation to initiate the cycle!
          </div>
        ) : (
          <div className="space-y-6">
            {reviews.map((rev) => (
              <div key={rev.id} className="glass-card space-y-6">
                <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 border-b border-white/10 pb-6">
                  <div>
                    <h2 className="text-xl font-bold text-white">Review Cycle: {rev.reviewCycle}</h2>
                    <p className="text-sm text-slate-400">Reviewer ID: <strong className="text-slate-300">{rev.reviewerUserId}</strong></p>
                  </div>
                  <div className="flex gap-4">
                    <div className="flex items-center gap-2 bg-slate-900 px-4 py-2 rounded-xl border border-white/5">
                      <span className="text-[10px] font-bold text-slate-400 uppercase">Self</span>
                      <span className="text-lg font-extrabold text-teal-400 flex items-center gap-1">
                        {rev.selfRating} <Star className="w-4 h-4 fill-teal-400 text-teal-400" />
                      </span>
                    </div>
                    <div className="flex items-center gap-2 bg-slate-900 px-4 py-2 rounded-xl border border-white/5">
                      <span className="text-[10px] font-bold text-slate-400 uppercase">Mgr</span>
                      <span className="text-lg font-extrabold text-amber-400 flex items-center gap-1">
                        {rev.managerRating} <Star className="w-4 h-4 fill-amber-400 text-amber-400" />
                      </span>
                    </div>
                  </div>
                </div>

                <div className="space-y-3">
                  <h3 className="text-xs font-bold text-slate-400 uppercase tracking-wider">Evaluation Comments</h3>
                  <p className="p-4 rounded-xl bg-slate-800/80 border border-white/5 text-sm text-slate-300 italic">
                    "{rev.comments}"
                  </p>
                </div>
              </div>
            ))}
          </div>
        )
      )}

      {/* Goal Modal */}
      {showGoalModal && (
        <div className="modal-backdrop">
          <div className="modal-panel">
            <div className="modal-header">
              <h3 className="text-lg font-bold text-slate-100">Add Goal / OKR</h3>
              <button onClick={() => setShowGoalModal(false)} className="text-slate-400 hover:text-white">&times;</button>
            </div>
            <form onSubmit={handleCreateGoal}>
              <div className="modal-body space-y-4">
                <div className="form-group">
                  <label className="form-label">Goal Title</label>
                  <input type="text" className="form-control" value={goalTitle} onChange={(e) => setGoalTitle(e.target.value)} required placeholder="e.g. Complete Leadership Training" />
                </div>
                <div className="form-group">
                  <label className="form-label">Goal Description</label>
                  <textarea className="form-control" value={goalDesc} onChange={(e) => setGoalDesc(e.target.value)} required placeholder="Describe what needs to be achieved..." />
                </div>
                <div className="grid grid-cols-2 gap-4">
                  <div className="form-group">
                    <label className="form-label">Target Date</label>
                    <input type="date" className="form-control" value={goalTargetDate} onChange={(e) => setGoalTargetDate(e.target.value)} required />
                  </div>
                  <div className="form-group">
                    <label className="form-label">Metric Target</label>
                    <input type="number" className="form-control" value={goalMetricTarget} onChange={(e) => setGoalMetricTarget(e.target.value)} required placeholder="e.g. 100" />
                  </div>
                </div>
              </div>
              <div className="modal-footer">
                <button type="button" className="btn-ghost" onClick={() => setShowGoalModal(false)}>Cancel</button>
                <button type="submit" className="btn-teal">Create OKR</button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Review Modal */}
      {showReviewModal && (
        <div className="modal-backdrop">
          <div className="modal-panel">
            <div className="modal-header">
              <h3 className="text-lg font-bold text-slate-100">Submit Performance Review</h3>
              <button onClick={() => setShowReviewModal(false)} className="text-slate-400 hover:text-white">&times;</button>
            </div>
            <form onSubmit={handleSubmitReview}>
              <div className="modal-body space-y-4">
                <div className="grid grid-cols-2 gap-4">
                  <div className="form-group">
                    <label className="form-label">Review Cycle</label>
                    <select className="form-control" value={reviewCycle} onChange={(e) => setReviewCycle(e.target.value)}>
                      <option value="Q2 2026">Q2 2026</option>
                      <option value="Q3 2026">Q3 2026</option>
                      <option value="Q4 2026">Q4 2026</option>
                      <option value="Annual 2026">Annual 2026</option>
                    </select>
                  </div>
                  <div className="form-group">
                    <label className="form-label">Reviewer User ID</label>
                    <input type="text" className="form-control" value={reviewerId} onChange={(e) => setReviewerId(e.target.value)} required />
                  </div>
                </div>
                <div className="grid grid-cols-2 gap-4">
                  <div className="form-group">
                    <label className="form-label">Self Rating (1.0 - 5.0)</label>
                    <input type="number" step="0.1" min="1" max="5" className="form-control" value={selfRating} onChange={(e) => setSelfRating(e.target.value)} required />
                  </div>
                  <div className="form-group">
                    <label className="form-label">Manager Rating (1.0 - 5.0)</label>
                    <input type="number" step="0.1" min="1" max="5" className="form-control" value={managerRating} onChange={(e) => setManagerRating(e.target.value)} required disabled={activeRole === 'Employee'} />
                  </div>
                </div>
                <div className="form-group">
                  <label className="form-label">Summary Comments</label>
                  <textarea className="form-control" rows="3" value={reviewComments} onChange={(e) => setReviewComments(e.target.value)} required placeholder="Provide self reflection and achievements..." />
                </div>
              </div>
              <div className="modal-footer">
                <button type="button" className="btn-ghost" onClick={() => setShowReviewModal(false)}>Cancel</button>
                <button type="submit" className="btn-teal">Submit Evaluation</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
