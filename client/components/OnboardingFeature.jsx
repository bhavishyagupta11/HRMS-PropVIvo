'use client';

import React, { useState } from 'react';
import { Layers, CheckCircle2, Circle, Clock, Play, UserCheck, Ticket, User } from 'lucide-react';
import { useRole } from '../context/RoleContext';

export default function OnboardingFeature({ onComplete }) {
  const { activeRole, graphqlFetch, token } = useRole();
  const [activeTab, setActiveTab] = useState('tasks'); // tasks, messages, relocation, team, training

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const [employeeInfo, setEmployeeInfo] = useState({
    name: 'Current User',
    designation: '--',
    department: '--',
    manager: '--',
    buddy: '--',
    joiningDate: '--',
    overallProgress: 0,
    isCompleted: false
  });

  const [tasks, setTasks] = useState([]);
  const [welcomeMessages, setWelcomeMessages] = useState([]);
  const [relocation, setRelocation] = useState({
    visaStatus: '--',
    accommodation: '--',
    travel: '--',
    allowance: '--',
    localBuddy: '--',
    tickets: []
  });
  const [teamIntros, setTeamIntros] = useState([]);
  const [onboardingTraining, setOnboardingTraining] = useState([]);
  const [onboardingMilestones, setOnboardingMilestones] = useState([]);

  const loadData = async () => {
    if (!token) return;
    setLoading(true);
    setError(null);
    try {
      const q1 = `
        query {
          myOnboarding {
            id
            designation
            department
            managerName
            buddyName
            joiningDate
            overallProgressPercent
            isCompleted
          }
        }
      `;
      const d1 = await graphqlFetch(q1);
      if (d1?.myOnboarding) {
        setEmployeeInfo({
          name: 'Current User',
          designation: d1.myOnboarding.designation,
          department: d1.myOnboarding.department,
          manager: d1.myOnboarding.managerName,
          buddy: d1.myOnboarding.buddyName,
          joiningDate: new Date(d1.myOnboarding.joiningDate).toLocaleDateString(),
          overallProgress: d1.myOnboarding.overallProgressPercent,
          isCompleted: d1.myOnboarding.isCompleted
        });
        
        const q2 = `
          query GetOthers($empId: String!) {
            myOnboardingTasks(onboardingEmployeeId: $empId) { id title description phase priority dueDate assignee status completedDate }
            myWelcomeMessages(onboardingEmployeeId: $empId) { id senderName senderRole message videoUrl }
            myRelocationSupport(onboardingEmployeeId: $empId) { relocationStatus visaStatus accommodation travelDetails allowance localBuddyContact supportTickets }
            myTeamIntroductions(onboardingEmployeeId: $empId) { id teamMemberName bio expertise funFact introductionStatus }
            myOnboardingMilestones(onboardingEmployeeId: $empId) { id title type scheduledDate isCompleted }
            myOnboardingTrainingModules(onboardingEmployeeId: $empId) { id title isMandatory progressPercent hasCertificate }
          }
        `;
        const d2 = await graphqlFetch(q2, { empId: d1.myOnboarding.id });
        if (d2) {
          setTasks((d2.myOnboardingTasks || []).map(t => ({
             ...t, dueDate: new Date(t.dueDate).toLocaleDateString(), completedDate: t.completedDate ? new Date(t.completedDate).toLocaleDateString() : null
          })));
          setWelcomeMessages((d2.myWelcomeMessages || []).map(m => ({
            id: m.id, sender: m.senderName, role: m.senderRole, message: m.message, hasVideo: !!m.videoUrl, read: false
          })));
          if (d2.myRelocationSupport) {
            setRelocation({
               visaStatus: d2.myRelocationSupport.visaStatus,
               accommodation: d2.myRelocationSupport.accommodation,
               travel: d2.myRelocationSupport.travelDetails,
               allowance: d2.myRelocationSupport.allowance,
               localBuddy: d2.myRelocationSupport.localBuddyContact,
               tickets: (d2.myRelocationSupport.supportTickets || []).map((t, i) => ({ id: i, issue: t, status: 'open' }))
            });
          }
          setTeamIntros((d2.myTeamIntroductions || []).map(t => ({
             id: t.id, name: t.teamMemberName, role: t.expertise, expertise: t.expertise, funFact: t.funFact, avatar: t.teamMemberName.substring(0, 2), connected: t.introductionStatus === 'connected'
          })));
          setOnboardingMilestones((d2.myOnboardingMilestones || []).map(m => ({
            id: m.id, title: m.title, date: new Date(m.scheduledDate).toLocaleDateString(), status: m.isCompleted ? 'completed' : 'scheduled'
          })));
          setOnboardingTraining((d2.myOnboardingTrainingModules || []).map(tr => ({
            id: tr.id, title: tr.title, type: tr.isMandatory ? 'Mandatory' : 'Optional', progress: tr.progressPercent, status: tr.progressPercent === 100 ? 'completed' : (tr.progressPercent > 0 ? 'in-progress' : 'not-started')
          })));
        }
      }
    } catch(err) {
      setError(err.message || 'Error loading onboarding data');
    } finally {
      setLoading(false);
    }
  };

  React.useEffect(() => {
    loadData();
  }, [token, activeRole]);

  const handleMarkComplete = async (id) => {
    try {
      const mutation = `mutation { completeOnboardingTask(taskId: "${id}") }`;
      const res = await graphqlFetch(mutation);
      if (res && res.completeOnboardingTask) {
        await loadData();
      }
    } catch (err) {
      alert(`Error completing task: ${err.message}`);
    }
  };

  const handleCompleteOnboarding = async () => {
    try {
      const mutation = `mutation { completeOnboarding }`;
      const res = await graphqlFetch(mutation);
      if (res && res.completeOnboarding) {
        if (onComplete) onComplete();
      }
    } catch (err) {
      alert(`Error completing onboarding: ${err.message}`);
    }
  };

  const phases = ['pre-joining', 'day-1', 'week-1', 'week-2', 'month-1'];

  return (
    <div className="animate-fade-in space-y-6">
      <div className="section-header">
        <div>
          <h1 className="section-title"><Layers className="w-7 h-7 text-teal-500" /> New Joiner Onboarding</h1>
          <p className="section-subtitle">Structured multi-phase onboarding with interactive milestones and relocation support</p>
        </div>
      </div>

      {/* Top Employee Info Card */}
      <div className="glass-card space-y-4">
        <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 border-b border-white/10 pb-4">
          <div className="flex items-center gap-4">
            <div className="avatar avatar-lg bg-teal-600 text-white font-bold">AJ</div>
            <div>
              <h2 className="text-xl font-bold text-white">{employeeInfo.name}</h2>
              <p className="text-sm text-slate-400">{employeeInfo.designation} · {employeeInfo.department}</p>
            </div>
          </div>
          <div className="flex flex-wrap gap-6 text-sm text-slate-300">
            <div><span className="text-slate-500 text-xs block">Manager</span> <strong>{employeeInfo.manager}</strong></div>
            <div><span className="text-slate-500 text-xs block">Onboarding Buddy</span> <strong>{employeeInfo.buddy}</strong></div>
            <div><span className="text-slate-500 text-xs block">Joining Date</span> <strong>{employeeInfo.joiningDate}</strong></div>
          </div>
        </div>

        {/* Overall Progress */}
        <div>
          <div className="flex justify-between text-xs font-semibold text-slate-300 mb-1.5">
            <span>Overall Onboarding Progress</span>
            <span className="text-teal-400 font-bold">{employeeInfo.overallProgress}%</span>
          </div>
          <div className="progress-bar bg-slate-900 h-2.5">
            <div className="progress-fill progress-teal" style={{ width: `${employeeInfo.overallProgress}%` }}></div>
          </div>
        </div>
      </div>

      {/* Tabs */}
      <div className="tab-bar">
        <button className={`tab-btn ${activeTab === 'tasks' ? 'active' : ''}`} onClick={() => setActiveTab('tasks')}>Tasks & Phases</button>
        <button className={`tab-btn ${activeTab === 'messages' ? 'active' : ''}`} onClick={() => setActiveTab('messages')}>Welcome Messages</button>
        <button className={`tab-btn ${activeTab === 'relocation' ? 'active' : ''}`} onClick={() => setActiveTab('relocation')}>Relocation Support</button>
        <button className={`tab-btn ${activeTab === 'training' ? 'active' : ''}`} onClick={() => setActiveTab('training')}>Training & Milestones</button>
        <button className={`tab-btn ${activeTab === 'team' ? 'active' : ''}`} onClick={() => setActiveTab('team')}>Team Introductions</button>
      </div>

      {/* Tab 1: Tasks */}
      {activeTab === 'tasks' && (
        <div className="space-y-6">
          {phases.map(phase => {
            const phaseTasks = tasks.filter(t => t.phase === phase);
            return (
              <div key={phase} className="glass-card space-y-4">
                <h3 className="font-bold text-white text-base uppercase tracking-wide border-b border-white/10 pb-2 flex items-center justify-between">
                  <span>Phase: {phase.replace('-', ' ')}</span>
                  <span className="badge badge-slate">{phaseTasks.length} Tasks</span>
                </h3>
                <div className="space-y-3">
                  {phaseTasks.map(task => (
                    <div key={task.id} className="p-4 rounded-xl bg-slate-900/60 border border-white/5 flex flex-col md:flex-row md:items-center justify-between gap-4">
                      <div>
                        <div className="flex items-center gap-2 mb-1">
                          <h4 className="font-bold text-white text-sm">{task.title}</h4>
                          <span className={`badge ${task.priority === 'high' ? 'badge-danger' : 'badge-warning'}`}>{task.priority}</span>
                          <span className="badge badge-slate">Assignee: {task.assignee}</span>
                        </div>
                        <p className="text-xs text-slate-400 mb-1">{task.desc}</p>
                        <div className="text-[11px] text-slate-500">Due: {task.dueDate}</div>
                      </div>
                      <div>
                        {task.status === 'completed' ? (
                          <div className="flex items-center gap-1 text-xs font-semibold text-emerald-400 bg-emerald-500/10 py-1.5 px-3 rounded-xl border border-emerald-500/20">
                            <CheckCircle2 className="w-4 h-4" /> Completed {task.completedDate}
                          </div>
                        ) : task.assignee === 'employee' ? (
                          <button className="btn-teal py-1.5 px-3 text-xs" onClick={() => handleMarkComplete(task.id)}>
                            Mark Complete
                          </button>
                        ) : (
                          <span className="badge badge-warning">Waiting on {task.assignee.toUpperCase()}</span>
                        )}
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            );
          })}

          {employeeInfo.overallProgress >= 60 && !employeeInfo.isCompleted && (
            <div className="text-center py-4">
              <button onClick={handleCompleteOnboarding} className="btn-teal py-3 px-8 text-base font-bold shadow-lg shadow-teal-900/50">
                🎉 Mark Onboarding Fully Complete
              </button>
            </div>
          )}
        </div>
      )}

      {/* Tab 2: Welcome Messages */}
      {activeTab === 'messages' && (
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          {welcomeMessages.map(msg => (
            <div key={msg.id} className="glass-card flex flex-col justify-between space-y-4">
              <div>
                <div className="flex items-center justify-between mb-3 border-b border-white/10 pb-3">
                  <div className="flex items-center gap-3">
                    <div className="avatar avatar-md bg-gradient-to-tr from-teal-600 to-orange-500 text-white">
                      {msg.sender.split(' ').map(n => n[0]).join('')}
                    </div>
                    <div>
                      <h4 className="font-bold text-white text-base">{msg.sender}</h4>
                      <div className="text-xs text-slate-400">{msg.role}</div>
                    </div>
                  </div>
                  <div className="flex items-center gap-2">
                    {msg.hasVideo && <span className="badge badge-orange"><Play className="w-3 h-3 fill-orange-400 mr-1" /> Video</span>}
                    <span className={`badge ${msg.read ? 'badge-success' : 'badge-warning'}`}>{msg.read ? 'Acknowledged' : 'New'}</span>
                  </div>
                </div>
                <p className="text-sm text-slate-200 leading-relaxed italic">"{msg.message}"</p>
              </div>
              <div className="flex flex-col gap-2 mt-2">
                {msg.hasVideo && (
                  <button className="btn-orange w-full py-2 justify-center text-xs">
                    <Play className="w-4 h-4 fill-white" /> Play Video Welcome
                  </button>
                )}
                {!msg.read ? (
                  <button
                    onClick={() => {
                      setWelcomeMessages(prev => prev.map(m => m.id === msg.id ? { ...m, read: true } : m));
                      setEmployeeInfo(prev => ({ ...prev, overallProgress: Math.min(100, prev.overallProgress + 4) }));
                    }}
                    className="btn-teal w-full py-2.5 justify-center text-xs"
                  >
                    Mark as Read & Acknowledged
                  </button>
                ) : (
                  <div className="text-center text-xs text-emerald-400 font-semibold py-1">✓ Acknowledged</div>
                )}
              </div>
            </div>
          ))}
        </div>
      )}

      {/* Tab 3: Relocation Support */}
      {activeTab === 'relocation' && (
        <div className="glass-card space-y-6">
          <h2 className="text-lg font-bold text-slate-100 mb-4">Relocation & Logistics Overview</h2>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
            <div className="p-4 rounded-xl bg-slate-900/50 border border-white/5">
              <div className="text-xs text-slate-400 uppercase font-semibold mb-1">Visa Status</div>
              <div className="font-bold text-teal-400 text-base">{relocation.visaStatus}</div>
            </div>
            <div className="p-4 rounded-xl bg-slate-900/50 border border-white/5">
              <div className="text-xs text-slate-400 uppercase font-semibold mb-1">Accommodation</div>
              <div className="font-bold text-white text-base">{relocation.accommodation}</div>
            </div>
            <div className="p-4 rounded-xl bg-slate-900/50 border border-white/5">
              <div className="text-xs text-slate-400 uppercase font-semibold mb-1">Travel / Flights</div>
              <div className="font-bold text-white text-base">{relocation.travel}</div>
            </div>
            <div className="p-4 rounded-xl bg-slate-900/50 border border-white/5">
              <div className="text-xs text-slate-400 uppercase font-semibold mb-1">Allowance</div>
              <div className="font-bold text-emerald-400 text-base">{relocation.allowance}</div>
            </div>
          </div>

          <div className="p-4 rounded-xl bg-teal-500/10 border border-teal-500/20 flex items-center justify-between">
            <div className="flex items-center gap-3">
              <UserCheck className="w-6 h-6 text-teal-400" />
              <div>
                <div className="font-bold text-white text-sm">Your Local Office Buddy</div>
                <div className="text-xs text-slate-400">{relocation.localBuddy}</div>
              </div>
            </div>
            <button className="btn-teal py-1.5 px-4 text-xs">Reach Out</button>
          </div>

          <div>
            <h3 className="text-sm font-bold text-slate-300 uppercase tracking-wider mb-3">Support Tickets</h3>
            <div className="space-y-2">
              {relocation.tickets.map(t => (
                <div key={t.id} className="p-3.5 rounded-xl bg-slate-900/60 border border-white/5 flex items-center justify-between">
                  <span className="font-medium text-white text-sm">{t.issue} (ID: {t.id})</span>
                  <span className={`badge ${t.status === 'open' ? 'badge-warning' : 'badge-success'}`}>{t.status.toUpperCase()}</span>
                </div>
              ))}
            </div>
          </div>
        </div>
      )}

      {/* Tab 4: Training & Milestones */}
      {activeTab === 'training' && (
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 animate-fade-in">
          {/* Mandatory & Optional Trainings */}
          <div className="glass-card space-y-4">
            <h3 className="font-bold text-white text-base border-b border-white/10 pb-3">Training Modules</h3>
            <div className="space-y-4">
              {onboardingTraining.map(tr => (
                <div key={tr.id} className="p-4 rounded-xl bg-slate-900/60 border border-white/5 space-y-3">
                  <div className="flex justify-between items-start">
                    <div>
                      <h4 className="font-bold text-white text-sm">{tr.title}</h4>
                      <span className={`badge text-[9px] mt-1 ${tr.type === 'Mandatory' ? 'badge-danger' : 'badge-slate'}`}>{tr.type}</span>
                    </div>
                    <span className={`badge ${tr.status === 'completed' ? 'badge-success' : tr.status === 'in-progress' ? 'badge-teal' : 'badge-slate'}`}>
                      {tr.status.replace('-', ' ').toUpperCase()}
                    </span>
                  </div>
                  <div className="space-y-1">
                    <div className="flex justify-between text-[11px] text-slate-400">
                      <span>Progress</span>
                      <span>{tr.progress}%</span>
                    </div>
                    <div className="progress-bar h-1.5 bg-slate-900">
                      <div className="progress-fill progress-teal" style={{ width: `${tr.progress}%` }}></div>
                    </div>
                  </div>
                  {tr.status !== 'completed' && (
                    <button
                      onClick={() => {
                        setOnboardingTraining(prev => prev.map(p => p.id === tr.id ? { ...p, progress: 100, status: 'completed' } : p));
                        setEmployeeInfo(prev => ({ ...prev, overallProgress: Math.min(100, prev.overallProgress + 10) }));
                      }}
                      className="btn-teal w-full py-1 text-[11px] justify-center mt-2"
                    >
                      Complete Module
                    </button>
                  )}
                </div>
              ))}
            </div>
          </div>

          {/* Onboarding Milestones */}
          <div className="glass-card space-y-4">
            <h3 className="font-bold text-white text-base border-b border-white/10 pb-3">Onboarding Milestones</h3>
            <div className="space-y-3">
              {onboardingMilestones.map(ms => (
                <div key={ms.id} className="p-4 rounded-xl bg-slate-900/60 border border-white/5 flex items-center justify-between">
                  <div>
                    <h4 className="font-bold text-white text-sm">{ms.title}</h4>
                    <p className="text-[11px] text-slate-400 mt-0.5">Scheduled: {ms.date}</p>
                  </div>
                  <span className={`badge ${ms.status === 'completed' ? 'badge-success' : 'badge-warning'}`}>
                    {ms.status.toUpperCase()}
                  </span>
                </div>
              ))}
            </div>
          </div>
        </div>
      )}

      {/* Tab 5: Team Introductions */}
      {activeTab === 'team' && (
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
          {teamIntros.map(t => (
            <div key={t.id} className="glass-card space-y-4 flex flex-col justify-between">
              <div>
                <div className="flex items-center justify-between border-b border-white/10 pb-3 mb-3">
                  <div className="flex items-center gap-3">
                    <div className="avatar avatar-lg bg-orange-600 text-white">{t.avatar}</div>
                    <div>
                      <h4 className="font-bold text-white text-base">{t.name}</h4>
                      <div className="text-xs text-slate-400">{t.role}</div>
                    </div>
                  </div>
                  <span className={`badge ${t.connected ? 'badge-success' : 'badge-slate'}`}>{t.connected ? 'Connected' : 'Not Connected'}</span>
                </div>
                <div className="space-y-3">
                  <div>
                    <div className="text-xs font-semibold text-slate-400 uppercase mb-1">Expertise</div>
                    <div className="text-sm text-teal-400 font-medium">{t.expertise}</div>
                  </div>
                  <div className="p-3 rounded-xl bg-slate-900/50 border border-white/5">
                    <div className="text-xs font-semibold text-slate-400 uppercase mb-1">Fun Fact 💡</div>
                    <div className="text-sm text-slate-300 italic">"{t.funFact}"</div>
                  </div>
                </div>
              </div>
              <div className="pt-3 border-t border-white/5">
                {!t.connected ? (
                  <button
                    onClick={() => {
                      setTeamIntros(prev => prev.map(item => item.id === t.id ? { ...item, connected: true } : item));
                      setEmployeeInfo(prev => ({ ...prev, overallProgress: Math.min(100, prev.overallProgress + 4) }));
                    }}
                    className="btn-teal w-full py-2 justify-center text-xs"
                  >
                    Say Hi / Connect
                  </button>
                ) : (
                  <div className="text-center text-xs text-teal-400 font-semibold py-1.5 bg-teal-500/10 rounded-xl border border-teal-500/20">✓ Connected</div>
                )}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
