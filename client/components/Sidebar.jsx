'use client';

/**
 * Sidebar — Desktop navigation per PSD Section 3.2 role-specific navigation
 * Displays all modules the active role has access to per PSD Section 3.1 Role Capability Matrix.
 * Clean keyboard accessibility, zero layout overflow, full responsive collapse.
 */

import React from 'react';
import {
  Home, Clock, Target, BookOpen, Star,
  Users, Calendar, Briefcase, BarChart2, Megaphone,
  FileText, DollarSign, Award, Bot, Receipt, Layers
} from 'lucide-react';
import { useRole } from '../context/RoleContext';

const ALL_MODULES = [
  { id: 'home',          label: 'Dashboard',        icon: Home,      roles: ['Employee','Manager','Reporting Manager','HR','Admin'] },
  { id: 'onboarding',   label: 'Onboarding',        icon: Layers,    roles: ['Employee','Manager','Reporting Manager','HR','Admin'] },
  { id: 'attendance',   label: 'Attendance',         icon: Clock,     roles: ['Employee','Manager','Reporting Manager','HR','Admin'] },
  { id: 'leave',        label: 'Leave',              icon: Calendar,  roles: ['Employee','Manager','Reporting Manager','HR','Admin'] },
  { id: 'payroll',      label: 'Payroll',            icon: DollarSign,roles: ['Employee','Manager','Reporting Manager','HR','Admin'] },
  { id: 'documents',    label: 'Documents',          icon: FileText,  roles: ['Employee','Manager','Reporting Manager','HR','Admin'] },
  { id: 'expenses',     label: 'Expenses',           icon: Receipt,   roles: ['Employee','Manager','Reporting Manager','HR','Admin'] },
  { id: 'performance',  label: 'Performance & Goals',icon: Target,    roles: ['Employee','Manager','Reporting Manager','HR','Admin'] },
  { id: 'contributions',label: 'Contributions',      icon: Star,      roles: ['Employee','Manager','Reporting Manager','HR','Admin'] },
  { id: 'training',     label: 'Training',           icon: BookOpen,  roles: ['Employee','Manager','Reporting Manager','HR','Admin'] },
  { id: 'recruitment',  label: 'Recruitment',        icon: Briefcase, roles: ['HR','Admin'] },
  { id: 'recognition',  label: 'Recognition',        icon: Award,     roles: ['Employee','Manager','Reporting Manager','HR','Admin'] },
  { id: 'announcements',label: 'Announcements',      icon: Megaphone, roles: ['Employee','Manager','Reporting Manager','HR','Admin'] },
  { id: 'team',         label: 'Team Management',    icon: Users,     roles: ['Manager','Reporting Manager','HR','Admin'] },
  { id: 'analytics',    label: 'Analytics',          icon: BarChart2, roles: ['Employee','Manager','Reporting Manager','HR','Admin'] },
  { id: 'copilot',      label: 'HR Copilot AI',      icon: Bot,       roles: ['Employee','Manager','Reporting Manager','HR','Admin'] },
];

export default function Sidebar({ activeTab, onTabChange }) {
  const { activeRole } = useRole();
  const visibleModules = ALL_MODULES.filter(m => m.roles.includes(activeRole));

  return (
    <aside className="sidebar flex flex-col justify-between h-full bg-slate-900 border-r border-white/10 select-none flex-shrink-0 z-30 overflow-y-auto scrollbar-none">
      <div>
        <div className="px-4 py-3 mb-2 border-b border-white/5">
          <p className="text-[11px] font-extrabold text-slate-500 uppercase tracking-wider px-2">
            Navigation · {activeRole}
          </p>
        </div>
        <nav className="space-y-1 px-3">
          {visibleModules.map((mod) => {
            const Icon = mod.icon;
            const isActive = activeTab === mod.id;
            return (
              <button
                key={mod.id}
                onClick={() => onTabChange(mod.id)}
                className={`sidebar-item w-full text-left flex items-center gap-3.5 px-4 py-3 rounded-2xl font-semibold text-sm transition-all duration-150 cursor-pointer
                  ${isActive 
                    ? 'active bg-teal-600 text-white font-bold shadow-lg shadow-teal-500/25 translate-x-1' 
                    : 'text-slate-300 hover:bg-white/5 hover:text-white'}`}
                aria-label={mod.label}
                aria-current={isActive ? 'page' : undefined}
                tabIndex={0}
              >
                <Icon className={`w-5 h-5 flex-shrink-0 transition-transform ${isActive ? 'scale-110 text-white' : 'text-slate-400'}`} strokeWidth={isActive ? 2.5 : 2} />
                <span className="truncate">{mod.label}</span>
              </button>
            );
          })}
        </nav>
      </div>

      {/* Footer Branding */}
      <div className="p-4 m-3 bg-slate-950/50 rounded-2xl border border-white/5 text-center flex-shrink-0 hidden md:block">
        <p className="text-xs font-bold text-slate-400">PropVivo HRMS</p>
        <p className="text-[10px] text-slate-600 mt-0.5">Version 1.0 · Secured</p>
      </div>
    </aside>
  );
}
