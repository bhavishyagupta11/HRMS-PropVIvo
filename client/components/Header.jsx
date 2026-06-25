'use client';

/**
 * Header — PSD Section 3 RBAC & Section 4.15 Copilot + Full Notification System
 * Mobile-first: shows role switcher, copilot shortcut, user info, and full notifications panel.
 */

import React, { useMemo, useState } from 'react';
import { Bot, ChevronDown, Bell, Check, CheckCircle2, AlertCircle, RefreshCw, Layers, Calendar, DollarSign, Briefcase, Award, BookOpen, Receipt, Megaphone } from 'lucide-react';
import { useRole, ROLES } from '../context/RoleContext';

const ROLE_COLORS = {
  [ROLES.EMPLOYEE]: 'role-pill-employee',
  [ROLES.MANAGER]: 'role-pill-manager',
  [ROLES.REPORTING_MANAGER]: 'role-pill-manager',
  [ROLES.HR]: 'role-pill-hr',
  [ROLES.ADMIN]: 'role-pill-admin',
};

const ROLE_PERSONAS = {
  [ROLES.EMPLOYEE]: { name: 'Sarah Mitchell', avatar: 'SM' },
  [ROLES.MANAGER]: { name: 'Michael Chen', avatar: 'MC' },
  [ROLES.REPORTING_MANAGER]: { name: 'Arjun Mehta', avatar: 'AM' },
  [ROLES.HR]: { name: 'Elena Rostova', avatar: 'ER' },
  [ROLES.ADMIN]: { name: 'Marcus Vance', avatar: 'MV' },
};

const AVATAR_BG = {
  [ROLES.EMPLOYEE]: 'bg-teal-600',
  [ROLES.MANAGER]: 'bg-blue-600',
  [ROLES.REPORTING_MANAGER]: 'bg-indigo-600',
  [ROLES.HR]: 'bg-purple-600',
  [ROLES.ADMIN]: 'bg-orange-600',
};

// Initial Notification Database per PSD requirements
const INITIAL_NOTIFICATIONS = [
  { id: 1, title: 'Company Announcement', desc: 'New global hybrid work policy updated by HR.', type: 'announcements', priority: 'high', time: '10 mins ago', unread: true, icon: Megaphone, roles: [ROLES.EMPLOYEE, ROLES.MANAGER, ROLES.REPORTING_MANAGER, ROLES.HR, ROLES.ADMIN] },
  { id: 2, title: 'Leave Approval Pending', desc: 'David Smith requested 3 days of Casual Leave.', type: 'leave', priority: 'high', time: '1 hour ago', unread: true, icon: Calendar, roles: [ROLES.MANAGER, ROLES.REPORTING_MANAGER, ROLES.HR, ROLES.ADMIN] },
  { id: 3, title: 'Expense Claim Submitted', desc: 'Robert Johnson submitted client travel claim for $1,240.00.', type: 'expenses', priority: 'medium', time: '3 hours ago', unread: true, icon: Receipt, roles: [ROLES.MANAGER, ROLES.REPORTING_MANAGER, ROLES.HR, ROLES.ADMIN] },
  { id: 4, title: 'Recruitment Update', desc: 'Arjun Mehta completed Technical Interview for Staff Backend Engineer.', type: 'recruitment', priority: 'medium', time: '5 hours ago', unread: false, icon: Briefcase, roles: [ROLES.HR, ROLES.ADMIN] },
  { id: 5, title: 'Payroll Processed', desc: 'May 2026 payslip and tax documents are ready for download.', type: 'payroll', priority: 'low', time: '1 day ago', unread: false, icon: DollarSign, roles: [ROLES.EMPLOYEE, ROLES.MANAGER, ROLES.REPORTING_MANAGER, ROLES.HR, ROLES.ADMIN] },
  { id: 6, title: 'Training Course Assigned', desc: 'Global Anti-Bribery & Corruption compliance training is due.', type: 'training', priority: 'low', time: '2 days ago', unread: false, icon: BookOpen, roles: [ROLES.EMPLOYEE, ROLES.MANAGER, ROLES.REPORTING_MANAGER, ROLES.HR, ROLES.ADMIN] },
  { id: 7, title: 'Peer Recognition Received', desc: 'Aisha Khan gave you an Excellence appreciation star.', type: 'recognition', priority: 'low', time: '3 days ago', unread: false, icon: Award, roles: [ROLES.EMPLOYEE, ROLES.MANAGER, ROLES.REPORTING_MANAGER, ROLES.HR, ROLES.ADMIN] },
];

export default function Header({ onCopilotOpen, onNavigate }) {
  const { activeRole, setActiveRole, ROLES: rolesEnum } = useRole();
  const [roleDdOpen, setRoleDdOpen] = useState(false);
  const [notifOpen, setNotifOpen] = useState(false);
  const [notifications, setNotifications] = useState(INITIAL_NOTIFICATIONS);

  const persona = ROLE_PERSONAS[activeRole] || ROLE_PERSONAS[ROLES.EMPLOYEE];
  const avatarBg = AVATAR_BG[activeRole] || AVATAR_BG[ROLES.EMPLOYEE];
  const visibleNotifications = useMemo(
    () => notifications.filter(n => n.roles.includes(activeRole)),
    [activeRole, notifications]
  );

  const unreadCount = visibleNotifications.filter(n => n.unread).length;

  const handleMarkAsRead = (id, e) => {
    e.stopPropagation();
    setNotifications(prev => prev.map(n => n.id === id ? { ...n, unread: false } : n));
  };

  const handleMarkAllRead = (e) => {
    e.stopPropagation();
    setNotifications(prev => prev.map(n => ({ ...n, unread: false })));
  };

  return (
    <header className="top-header relative z-40">
      {/* Brand */}
      <div className="flex items-center gap-3 cursor-pointer" onClick={() => onNavigate && onNavigate('home')}>
        <div className="w-8 h-8 rounded-xl bg-gradient-to-br from-teal-600 to-teal-400 flex items-center justify-center flex-shrink-0 shadow-md shadow-teal-500/20">
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="white" strokeWidth="2.5">
            <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/>
            <circle cx="9" cy="7" r="4"/>
            <path d="M23 21v-2a4 4 0 0 0-3-3.87"/>
            <path d="M16 3.13a4 4 0 0 1 0 7.75"/>
          </svg>
        </div>
        <div>
          <div className="text-sm font-bold text-slate-100 leading-none">WorkFlow</div>
          <div className="text-[10px] text-slate-500 leading-none mt-0.5">Global HRMS</div>
        </div>
      </div>

      {/* Right side controls */}
      <div className="flex items-center gap-2">
        {/* Role Switcher — PSD Section 3 */}
        <div className="relative">
          <button
            onClick={() => { setRoleDdOpen(!roleDdOpen); setNotifOpen(false); }}
            className={`role-pill ${ROLE_COLORS[activeRole]}`}
            aria-expanded={roleDdOpen}
            aria-label="Toggle Role Access Control Switcher"
          >
            {activeRole}
            <ChevronDown className={`w-3 h-3 transition-transform ${roleDdOpen ? 'rotate-180' : ''}`} />
          </button>

          {roleDdOpen && (
            <div className="absolute right-0 top-full mt-2 bg-slate-900 border border-white/10 rounded-2xl shadow-2xl py-2 min-w-[150px] z-50 animate-fade-in backdrop-blur-2xl">
              <div className="px-4 py-1 text-[10px] font-bold text-slate-500 uppercase tracking-wider border-b border-white/10 mb-1">
                Active RBAC Role
              </div>
              {Object.values(rolesEnum).map((role) => (
                <button
                  key={role}
                  onClick={() => { setActiveRole(role); setRoleDdOpen(false); }}
                  className={`w-full text-left px-4 py-2.5 text-sm font-semibold transition-colors flex items-center justify-between
                    ${activeRole === role ? 'text-teal-400 bg-teal-500/10' : 'text-slate-300 hover:bg-white/5'}`}
                >
                  {role}
                  {activeRole === role && <span className="w-1.5 h-1.5 rounded-full bg-teal-400" />}
                </button>
              ))}
            </div>
          )}
        </div>

        {/* Full Notification System — PSD Section 4 */}
        <div className="relative">
          <button
            onClick={() => { setNotifOpen(!notifOpen); setRoleDdOpen(false); }}
            className={`w-9 h-9 rounded-xl flex items-center justify-center transition-colors relative border
              ${notifOpen ? 'bg-slate-800 border-white/20 text-white' : 'bg-white/5 border-white/5 text-slate-400 hover:text-slate-200 hover:bg-white/10'}`}
            aria-expanded={notifOpen}
            aria-label="View Notification System Panel"
          >
            <Bell className="w-4 h-4" />
            {unreadCount > 0 && (
              <span className="absolute -top-1 -right-1 min-w-[18px] h-[18px] bg-orange-500 text-white font-extrabold text-[10px] rounded-full flex items-center justify-center px-1 animate-pulse border border-slate-900">
                {unreadCount}
              </span>
            )}
          </button>

          {notifOpen && (
            <div className="absolute right-0 top-full mt-2 w-80 md:w-96 bg-slate-900 border border-white/10 rounded-3xl shadow-2xl z-50 animate-fade-in backdrop-blur-2xl overflow-hidden flex flex-col max-h-[550px]">
              {/* Notification Header */}
              <div className="p-4 bg-slate-950/60 border-b border-white/10 flex items-center justify-between flex-shrink-0">
                <div className="flex items-center gap-2">
                  <h3 className="font-bold text-white text-sm">Notifications</h3>
                  {unreadCount > 0 && (
                    <span className="px-2 py-0.5 bg-orange-500/20 text-orange-400 text-[10px] font-bold rounded-full border border-orange-500/30">
                      {unreadCount} unread
                    </span>
                  )}
                </div>
                {unreadCount > 0 && (
                  <button onClick={handleMarkAllRead} className="text-xs text-teal-400 hover:underline font-semibold flex items-center gap-1">
                    <Check className="w-3.5 h-3.5" /> Mark all read
                  </button>
                )}
              </div>

              {/* Notification Feed */}
              <div className="flex-1 overflow-y-auto divide-y divide-white/5 bg-slate-900/50">
                {visibleNotifications.length === 0 ? (
                  <div className="p-8 text-center text-slate-500 text-xs font-medium">
                    No new notifications.
                  </div>
                ) : (
                  visibleNotifications.map((n) => {
                    const Icon = n.icon;
                    return (
                      <div
                        key={n.id}
                        onClick={() => {
                          if (onNavigate) onNavigate(n.type);
                          setNotifOpen(false);
                          setNotifications(prev => prev.map(item => item.id === n.id ? { ...item, unread: false } : item));
                        }}
                        className={`p-4 transition-colors cursor-pointer flex gap-3.5 relative hover:bg-slate-800/50 ${n.unread ? 'bg-teal-500/5' : ''}`}
                      >
                        <div className={`w-10 h-10 rounded-2xl flex items-center justify-center flex-shrink-0 border ${
                          n.priority === 'high' ? 'bg-orange-500/10 text-orange-400 border-orange-500/20' :
                          n.priority === 'medium' ? 'bg-blue-500/10 text-blue-400 border-blue-500/20' :
                          'bg-slate-800 text-slate-400 border-white/5'
                        }`}>
                          <Icon className="w-5 h-5" />
                        </div>
                        <div className="flex-1 min-w-0">
                          <div className="flex items-center justify-between gap-2">
                            <p className={`text-xs font-bold truncate ${n.unread ? 'text-white' : 'text-slate-300'}`}>
                              {n.title}
                            </p>
                            <span className="text-[10px] text-slate-500 flex-shrink-0">{n.time}</span>
                          </div>
                          <p className="text-xs text-slate-400 mt-1 line-clamp-2 leading-relaxed">
                            {n.desc}
                          </p>
                          <div className="mt-2 flex items-center justify-between">
                            <span className={`text-[10px] uppercase tracking-wider font-bold px-1.5 py-0.5 rounded ${
                              n.priority === 'high' ? 'bg-orange-500/20 text-orange-300' :
                              n.priority === 'medium' ? 'bg-blue-500/20 text-blue-300' :
                              'bg-white/5 text-slate-400'
                            }`}>
                              {n.priority} priority
                            </span>
                            {n.unread && (
                              <button
                                onClick={(e) => handleMarkAsRead(n.id, e)}
                                className="text-[10px] text-teal-400 hover:underline font-semibold flex items-center gap-0.5"
                              >
                                Mark as read
                              </button>
                            )}
                          </div>
                        </div>
                      </div>
                    );
                  })
                )}
              </div>

              {/* Notification Footer */}
              <div className="p-3 bg-slate-950/60 border-t border-white/10 text-center flex-shrink-0">
                <p className="text-[11px] text-slate-400 font-medium">Click any notification to open its module</p>
              </div>
            </div>
          )}
        </div>

        {/* HR Copilot AI Shortcut — PSD 4.15 */}
        <button
          onClick={onCopilotOpen}
          className="w-9 h-9 rounded-xl bg-teal-600/20 hover:bg-teal-600/30 flex items-center justify-center text-teal-400 transition-colors border border-teal-500/20 shadow-sm"
          title="Open Embedded HR Copilot AI Assistant"
          aria-label="Open HR Copilot AI Assistant"
        >
          <Bot className="w-4 h-4" />
        </button>

        {/* User Avatar */}
        <div className={`avatar avatar-sm ${avatarBg} text-white font-bold ml-1 flex-shrink-0 shadow-md`} title={persona.name}>
          {persona.avatar}
        </div>
      </div>
    </header>
  );
}
