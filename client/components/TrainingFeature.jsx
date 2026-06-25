'use client';

import React, { useState } from 'react';
import { BookOpen, Play, FileText, ClipboardCheck, Zap, Download, CheckCircle2 } from 'lucide-react';

export default function TrainingFeature() {
  const [activeTab, setActiveTab] = useState('modules'); // modules, certificates
  const [statusFilter, setStatusFilter] = useState('all'); // all, not-started, in-progress, completed
  const [selectedMod, setSelectedMod] = useState(null);

  const [modules, setModules] = useState([
    { id: 'TRN-001', title: 'Company Orientation', category: 'orientation', duration: '2h 30m', dueDate: '2026-07-15', mandatory: true, status: 'completed', progress: 100, certificateEligible: true, certificateIssued: true, contents: [
      { id: 'C1', title: 'Welcome Video from CEO', type: 'video', duration: '15m', completed: true },
      { id: 'C2', title: 'Company Handbook', type: 'document', duration: '45m', completed: true },
      { id: 'C3', title: 'Culture & Values Quiz', type: 'quiz', duration: '20m', completed: true },
    ]},
    { id: 'TRN-002', title: 'React Advanced Patterns', category: 'technical', duration: '8h 00m', dueDate: '2026-08-31', mandatory: false, status: 'in-progress', progress: 45, certificateEligible: true, certificateIssued: false, contents: [
      { id: 'C4', title: 'Custom Hooks Deep Dive', type: 'video', duration: '90m', completed: true },
      { id: 'C5', title: 'Context & State Management', type: 'video', duration: '75m', completed: true },
      { id: 'C6', title: 'Performance Optimization', type: 'interactive', duration: '60m', completed: false },
      { id: 'C7', title: 'Testing React Apps', type: 'video', duration: '80m', completed: false },
      { id: 'C8', title: 'Final Assessment', type: 'quiz', duration: '30m', completed: false },
    ]},
    { id: 'TRN-003', title: 'Data Privacy & GDPR', category: 'compliance', duration: '1h 30m', dueDate: '2026-07-31', mandatory: true, status: 'not-started', progress: 0, certificateEligible: true, certificateIssued: false, contents: [
      { id: 'C9', title: 'GDPR Overview', type: 'document', duration: '30m', completed: false },
      { id: 'C10', title: 'Data Handling Practices', type: 'video', duration: '45m', completed: false },
      { id: 'C11', title: 'Compliance Quiz', type: 'quiz', duration: '15m', completed: false },
    ]},
    { id: 'TRN-004', title: 'Leadership Essentials', category: 'soft-skills', duration: '4h 00m', dueDate: '2026-09-30', mandatory: false, status: 'not-started', progress: 0, certificateEligible: false, certificateIssued: false, contents: [
      { id: 'C12', title: 'Communication Frameworks', type: 'video', duration: '90m', completed: false },
      { id: 'C13', title: 'Conflict Resolution', type: 'interactive', duration: '60m', completed: false },
    ]},
  ]);

  const handleToggleComplete = (modId, contentId) => {
    setModules(prev => prev.map(m => {
      if (m.id === modId) {
        const updatedContents = m.contents.map(c => c.id === contentId ? { ...c, completed: true } : c);
        const completedCount = updatedContents.filter(c => c.completed).length;
        const progress = Math.round((completedCount / updatedContents.length) * 100);
        const status = progress === 100 ? 'completed' : progress > 0 ? 'in-progress' : 'not-started';
        const certificateIssued = m.certificateEligible && progress === 100;
        const updatedMod = { ...m, contents: updatedContents, progress, status, certificateIssued };
        if (selectedMod && selectedMod.id === modId) setSelectedMod(updatedMod);
        return updatedMod;
      }
      return m;
    }));
  };

  const filteredModules = statusFilter === 'all' ? modules : modules.filter(m => m.status === statusFilter);
  const earnedCertificates = modules.filter(m => m.certificateIssued);

  const getIcon = (type) => {
    switch (type) {
      case 'video': return <Play className="w-4 h-4 text-orange-400 flex-shrink-0" />;
      case 'document': return <FileText className="w-4 h-4 text-blue-400 flex-shrink-0" />;
      case 'quiz': return <ClipboardCheck className="w-4 h-4 text-emerald-400 flex-shrink-0" />;
      case 'interactive': return <Zap className="w-4 h-4 text-amber-400 flex-shrink-0" />;
      default: return <FileText className="w-4 h-4 text-slate-400 flex-shrink-0" />;
    }
  };

  return (
    <div className="animate-fade-in space-y-6">
      <div className="section-header">
        <div>
          <h1 className="section-title"><BookOpen className="w-7 h-7 text-teal-500" /> Training & Development</h1>
          <p className="section-subtitle">Self-paced learning journeys, compliance certifications, and interactive courses</p>
        </div>
      </div>

      {/* Tabs */}
      <div className="tab-bar">
        <button className={`tab-btn ${activeTab === 'modules' ? 'active' : ''}`} onClick={() => setActiveTab('modules')}>Learning Modules</button>
        <button className={`tab-btn ${activeTab === 'certificates' ? 'active' : ''}`} onClick={() => setActiveTab('certificates')}>
          Earned Certificates ({earnedCertificates.length})
        </button>
      </div>

      {/* Tab 1: Modules */}
      {activeTab === 'modules' && (
        <div className="space-y-6">
          {/* Filter Status */}
          <div className="flex flex-wrap gap-2 p-2 bg-slate-900/50 rounded-2xl border border-white/10">
            {['all', 'not-started', 'in-progress', 'completed'].map(st => (
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

          {/* Module Grid */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {filteredModules.map((mod) => (
              <div key={mod.id} className="glass-card flex flex-col justify-between space-y-4">
                <div>
                  <div className="flex items-center justify-between mb-2">
                    <span className="badge badge-teal uppercase text-[10px]">{mod.category}</span>
                    <span className={`badge ${mod.mandatory ? 'badge-orange' : 'badge-slate'}`}>{mod.mandatory ? 'Mandatory' : 'Optional'}</span>
                  </div>
                  <h3 className="font-bold text-white text-lg mb-1">{mod.title}</h3>
                  <div className="text-xs text-slate-400 mb-4">Duration: <strong>{mod.duration}</strong> · Due: <strong>{mod.dueDate}</strong></div>

                  {/* Progress Bar */}
                  <div>
                    <div className="flex justify-between text-xs font-semibold text-slate-300 mb-1">
                      <span>Course Progress</span>
                      <span>{mod.progress}%</span>
                    </div>
                    <div className="progress-bar bg-slate-900">
                      <div className="progress-fill progress-teal" style={{ width: `${mod.progress}%` }}></div>
                    </div>
                  </div>
                </div>
                <div className="border-t border-white/10 pt-3 flex items-center justify-between">
                  <span className={`badge ${mod.status === 'completed' ? 'badge-success' : mod.status === 'in-progress' ? 'badge-teal' : 'badge-slate'}`}>
                    {mod.status.toUpperCase()}
                  </span>
                  <button className="btn-teal py-1.5 px-4 text-xs" onClick={() => setSelectedMod(mod)}>
                    {mod.status === 'completed' ? 'Review Content' : mod.status === 'in-progress' ? 'Continue Course' : 'Start Learning'}
                  </button>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* Tab 2: Certificates */}
      {activeTab === 'certificates' && (
        <div className="glass-card">
          <h2 className="text-lg font-bold text-slate-100 mb-4">Verified Professional Certifications</h2>
          {earnedCertificates.length === 0 ? (
            <div className="empty-state">
              <div className="empty-icon"><BookOpen className="w-8 h-8 text-slate-500" /></div>
              <h3 className="empty-title">No Certificates Yet</h3>
              <p className="empty-desc">Complete certificate-eligible training modules to earn digital credentials.</p>
            </div>
          ) : (
            <div className="space-y-3">
              {earnedCertificates.map((cert) => (
                <div key={cert.id} className="p-4 rounded-xl bg-slate-900/50 border border-white/10 flex items-center justify-between">
                  <div className="flex items-center gap-3">
                    <div className="w-10 h-10 rounded-xl bg-emerald-500/20 text-emerald-400 flex items-center justify-center font-extrabold text-lg">✓</div>
                    <div>
                      <div className="font-bold text-white text-sm">{cert.title} Certification</div>
                      <div className="text-xs text-slate-400 mt-0.5">Category: {cert.category} · Status: Active Credential</div>
                    </div>
                  </div>
                  <button className="btn-teal py-1.5 px-3 text-xs">
                    <Download className="w-3 h-3" /> Download Certificate
                  </button>
                </div>
              ))}
            </div>
          )}
        </div>
      )}

      {/* Module Detail Modal */}
      {selectedMod && (
        <div className="modal-backdrop">
          <div className="modal-panel max-w-xl">
            <div className="modal-header">
              <div>
                <h3 className="text-lg font-bold text-white">{selectedMod.title}</h3>
                <p className="text-xs text-teal-400 font-semibold">{selectedMod.category.toUpperCase()} · {selectedMod.duration}</p>
              </div>
              <button onClick={() => setSelectedMod(null)} className="text-slate-400 hover:text-white">&times;</button>
            </div>
            <div className="modal-body space-y-4">
              <div className="flex justify-between text-xs font-bold text-slate-300">
                <span>Course Chapters & Content Items</span>
                <span>{selectedMod.progress}% Complete</span>
              </div>
              <div className="progress-bar bg-slate-900 mb-4">
                <div className="progress-fill progress-teal" style={{ width: `${selectedMod.progress}%` }}></div>
              </div>
              <div className="space-y-3">
                {selectedMod.contents.map((item) => (
                  <div key={item.id} className="p-4 rounded-xl bg-slate-900/60 border border-white/5 flex items-center justify-between gap-4">
                    <div className="flex items-center gap-3">
                      {getIcon(item.type)}
                      <div>
                        <div className="font-bold text-white text-sm">{item.title}</div>
                        <div className="text-xs text-slate-400 uppercase font-semibold">{item.type} ({item.duration})</div>
                      </div>
                    </div>
                    <div>
                      {item.completed ? (
                        <span className="flex items-center gap-1 text-xs font-semibold text-emerald-400 bg-emerald-500/10 py-1 px-2.5 rounded-lg border border-emerald-500/20">
                          <CheckCircle2 className="w-3.5 h-3.5" /> Completed
                        </span>
                      ) : (
                        <button className="btn-teal py-1 px-3 text-xs" onClick={() => handleToggleComplete(selectedMod.id, item.id)}>
                          Mark Complete
                        </button>
                      )}
                    </div>
                  </div>
                ))}
              </div>
            </div>
            <div className="modal-footer">
              <button className="btn-ghost" onClick={() => setSelectedMod(null)}>Close Panel</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
