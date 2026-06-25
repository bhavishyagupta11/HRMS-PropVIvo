'use client';

import React, { useState } from 'react';
import { Megaphone, Eye, ThumbsUp, MessageSquare, CheckSquare, Plus, CheckCircle2 } from 'lucide-react';
import { useRole } from '../context/RoleContext';

export default function AnnouncementFeature() {
  const { capabilities } = useRole();
  const [showModal, setShowModal] = useState(false);
  const [categoryFilter, setCategoryFilter] = useState('all');

  const [announcements, setAnnouncements] = useState([
    { id: 'ANN-001', title: 'Annual Performance Reviews Start July 1', category: 'hr-update', priority: 'high', content: 'The annual performance review cycle for FY 2026-27 kicks off on July 1st. All employees must complete their self-assessment by July 15th. Manager reviews will be completed by July 31st.', scope: 'global', targetDept: null, publishedAt: 'June 20, 2026', expiryDate: '2026-07-31', views: 234, likes: 45, comments: 12, acknowledgments: 180, requiresAcknowledgment: true, acknowledged: false, author: 'HR Team', attachments: ['review-guidelines.pdf'] },
    { id: 'ANN-002', title: 'Company Offsite - August 12-14', category: 'event', priority: 'medium', content: 'We are excited to announce our annual company offsite at Coorg! This is a great opportunity for team bonding. Registration opens June 25th.', scope: 'global', targetDept: null, publishedAt: 'June 18, 2026', expiryDate: '2026-08-12', views: 312, likes: 128, comments: 45, acknowledgments: 0, requiresAcknowledgment: false, acknowledged: false, author: 'Admin Team', attachments: [] },
    { id: 'ANN-003', title: 'Updated Data Security Policy', category: 'compliance', priority: 'high', content: 'Effective July 1, 2026, all employees must complete the updated data security training. The new policy includes stricter password requirements and mandatory 2FA for all systems.', scope: 'global', targetDept: null, publishedAt: 'June 15, 2026', expiryDate: '2026-07-01', views: 289, likes: 32, comments: 18, acknowledgments: 201, requiresAcknowledgment: true, acknowledged: true, author: 'IT Security', attachments: ['security-policy-v2.pdf', 'training-schedule.pdf'] },
  ]);

  // Form state
  const [newTitle, setNewTitle] = useState('');
  const [newCategory, setNewCategory] = useState('hr-update');
  const [newPriority, setNewPriority] = useState('medium');
  const [newContent, setNewContent] = useState('');
  const [newScope, setNewScope] = useState('global');
  const [requiresAck, setRequiresAck] = useState(false);

  const handleAcknowledge = (id) => {
    setAnnouncements(prev => prev.map(a => {
      if (a.id === id) {
        return { ...a, acknowledged: true, acknowledgments: a.acknowledgments + 1 };
      }
      return a;
    }));
  };

  const handleCreateAnnounce = (e) => {
    e.preventDefault();
    if (!newTitle || !newContent) return;

    const newAnn = {
      id: `ANN-${Date.now().toString().slice(-3)}`,
      title: newTitle,
      category: newCategory,
      priority: newPriority,
      content: newContent,
      scope: newScope,
      targetDept: null,
      publishedAt: 'June 24, 2026',
      expiryDate: '2026-08-31',
      views: 1,
      likes: 0,
      comments: 0,
      acknowledgments: 0,
      requiresAcknowledgment: requiresAck,
      acknowledged: false,
      author: 'Admin Team',
      attachments: []
    };

    setAnnouncements([newAnn, ...announcements]);
    setShowModal(false);
    setNewTitle('');
    setNewContent('');
  };

  const getCatClass = (cat) => {
    switch(cat) {
      case 'hr-update': return 'badge-blue';
      case 'event': return 'badge-teal';
      case 'policy': return 'badge-orange';
      case 'celebration': return 'badge-success';
      case 'compliance': return 'badge-danger';
      default: return 'badge-slate';
    }
  };

  const filteredAnn = categoryFilter === 'all' ? announcements : announcements.filter(a => a.category === categoryFilter);

  return (
    <div className="animate-fade-in space-y-6">
      <div className="section-header">
        <div>
          <h1 className="section-title"><Megaphone className="w-7 h-7 text-teal-500" /> Company Announcements</h1>
          <p className="section-subtitle">Targeted company-wide broadcasts, event notifications, and compliance policy acknowledgments</p>
        </div>
        {capabilities.canCreateAnnouncement && (
          <button className="btn-teal py-3 px-5 text-sm" onClick={() => setShowModal(true)}>
            <Plus className="w-4 h-4" /> Create Announcement
          </button>
        )}
      </div>

      {/* Filter Bar */}
      <div className="flex flex-wrap gap-2 p-2 bg-slate-900/50 rounded-2xl border border-white/10">
        {['all', 'hr-update', 'event', 'policy', 'compliance'].map(cat => (
          <button
            key={cat}
            className={`px-4 py-2 rounded-xl text-xs font-bold uppercase tracking-wider transition-colors ${
              categoryFilter === cat ? 'bg-teal-600 text-white' : 'text-slate-400 hover:bg-white/5'
            }`}
            onClick={() => setCategoryFilter(cat)}
          >
            {cat.replace('-', ' ')}
          </button>
        ))}
      </div>

      {/* Announcement List */}
      <div className="max-w-4xl mx-auto space-y-6">
        {filteredAnn.map((ann) => (
          <div key={ann.id} className="glass-card space-y-4">
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-2 border-b border-white/10 pb-4">
              <div>
                <div className="flex items-center gap-2 mb-1">
                  <span className={`w-2.5 h-2.5 rounded-full ${ann.priority === 'high' ? 'bg-red-500' : ann.priority === 'medium' ? 'bg-yellow-500' : 'bg-green-500'}`} title={`Priority: ${ann.priority}`} />
                  <h3 className="font-bold text-white text-lg">{ann.title}</h3>
                </div>
                <div className="text-xs text-slate-400">Posted by <strong className="text-slate-300">{ann.author}</strong> · {ann.publishedAt} · Scope: <strong className="text-teal-400 uppercase">{ann.scope}</strong></div>
              </div>
              <span className={`badge ${getCatClass(ann.category)} uppercase text-xs self-start md:self-center`}>
                {ann.category.replace('-', ' ')}
              </span>
            </div>

            <p className="text-sm text-slate-200 leading-relaxed bg-slate-900/40 p-4 rounded-xl border border-white/5">
              {ann.content}
            </p>

            {ann.attachments.length > 0 && (
              <div className="flex items-center gap-2 text-xs text-slate-300 bg-slate-800/80 p-2.5 rounded-xl border border-white/5">
                <strong className="text-slate-400">Attachments:</strong>
                {ann.attachments.map((att, i) => <a key={i} href="#" className="text-teal-400 font-semibold hover:underline">📎 {att}</a>)}
              </div>
            )}

            <div className="border-t border-white/10 pt-4 flex flex-col md:flex-row md:items-center justify-between gap-4">
              <div className="flex items-center gap-4 text-xs text-slate-400">
                <span className="flex items-center gap-1.5"><Eye className="w-3.5 h-3.5 text-slate-500" /> {ann.views} Views</span>
                <span className="flex items-center gap-1.5"><ThumbsUp className="w-3.5 h-3.5 text-teal-500" /> {ann.likes} Likes</span>
                <span className="flex items-center gap-1.5"><MessageSquare className="w-3.5 h-3.5 text-blue-500" /> {ann.comments} Comments</span>
                {ann.requiresAcknowledgment && (
                  <span className="flex items-center gap-1.5 text-amber-400"><CheckSquare className="w-3.5 h-3.5" /> {ann.acknowledgments} Acknowledged</span>
                )}
              </div>

              {ann.requiresAcknowledgment && (
                <div>
                  {ann.acknowledged ? (
                    <span className="inline-flex items-center gap-1 text-xs font-bold text-emerald-400 bg-emerald-500/10 py-1.5 px-3 rounded-xl border border-emerald-500/20">
                      <CheckCircle2 className="w-4 h-4" /> You Acknowledged Policy
                    </span>
                  ) : (
                    <button className="btn-orange py-1.5 px-4 text-xs font-bold" onClick={() => handleAcknowledge(ann.id)}>
                      Acknowledge Policy Compliance
                    </button>
                  )}
                </div>
              )}
            </div>
          </div>
        ))}
      </div>

      {/* Create Announcement Modal */}
      {showModal && (
        <div className="modal-backdrop">
          <div className="modal-panel max-w-lg">
            <div className="modal-header">
              <h3 className="text-lg font-bold text-white">Broadcast Announcement</h3>
              <button onClick={() => setShowModal(false)} className="text-slate-400 hover:text-white">&times;</button>
            </div>
            <form onSubmit={handleCreateAnnounce}>
              <div className="modal-body space-y-4">
                <div className="form-group">
                  <label className="form-label">Announcement Title</label>
                  <input type="text" className="form-control" placeholder="e.g. Annual Offsite Registration" value={newTitle} onChange={(e) => setNewTitle(e.target.value)} required />
                </div>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div className="form-group mb-0">
                    <label className="form-label">Category</label>
                    <select className="form-control" value={newCategory} onChange={(e) => setNewCategory(e.target.value)}>
                      <option value="hr-update" className="bg-slate-900">HR Update</option>
                      <option value="event" className="bg-slate-900">Event</option>
                      <option value="policy" className="bg-slate-900">Policy</option>
                      <option value="compliance" className="bg-slate-900">Compliance</option>
                    </select>
                  </div>
                  <div className="form-group mb-0">
                    <label className="form-label">Priority</label>
                    <select className="form-control" value={newPriority} onChange={(e) => setNewPriority(e.target.value)}>
                      <option value="high" className="bg-slate-900">High</option>
                      <option value="medium" className="bg-slate-900">Medium</option>
                      <option value="low" className="bg-slate-900">Low</option>
                    </select>
                  </div>
                </div>
                <div className="form-group">
                  <label className="form-label">Scope</label>
                  <select className="form-control" value={newScope} onChange={(e) => setNewScope(e.target.value)}>
                    <option value="global" className="bg-slate-900">Global (Company-wide)</option>
                    <option value="department" className="bg-slate-900">Department Specific</option>
                  </select>
                </div>
                <div className="form-group">
                  <label className="form-label">Announcement Content</label>
                  <textarea className="form-control" rows="4" placeholder="Provide full broadcast message..." value={newContent} onChange={(e) => setNewContent(e.target.value)} required></textarea>
                </div>
                <label className="flex items-center gap-3 p-3.5 rounded-xl bg-slate-900/50 border border-white/10 cursor-pointer hover:bg-slate-900">
                  <input type="checkbox" checked={requiresAck} onChange={(e) => setRequiresAck(e.target.checked)} className="accent-teal-500 w-4 h-4" />
                  <div>
                    <div className="font-semibold text-sm text-white">Require Compliance Acknowledgment</div>
                    <div className="text-xs text-slate-400 mt-0.5">Employees must explicitly acknowledge reading this broadcast</div>
                  </div>
                </label>
              </div>
              <div className="modal-footer">
                <button type="button" className="btn-ghost" onClick={() => setShowModal(false)}>Cancel</button>
                <button type="submit" className="btn-teal">Publish Broadcast</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
