'use client';

/**
 * HomeDashboard — PSD Section 2.1 Product Goals & Section 3 RBAC
 * Implements 100% distinct, role-tailored dashboards for Employee, Manager, HR, and Admin.
 * Zero identical cards reused across roles. Fully responsive across 320px to 1920px.
 */

import React from 'react';
import {
  Clock, Calendar, DollarSign, FileText, Target, BookOpen,
  Star, Award, Megaphone, BarChart2, Users, Briefcase, Layers, Receipt,
  CheckCircle, AlertCircle, TrendingUp, Shield, Settings, FileCheck, UserCheck, Activity, Database, Key, Check, X, Bot
} from 'lucide-react';
import { useRole, ROLES } from '../context/RoleContext';

const PERSONA_INFO = {
  [ROLES.EMPLOYEE]: { name: 'Sarah Mitchell', title: 'Senior Software Engineer', dept: 'Engineering', location: 'New York, USA' },
  [ROLES.MANAGER]:  { name: 'Michael Chen', title: 'Engineering Manager', dept: 'Engineering', location: 'New York, USA' },
  [ROLES.REPORTING_MANAGER]: { name: 'Arjun Mehta', title: 'Reporting Manager', dept: 'Engineering', location: 'Bangalore, India' },
  [ROLES.HR]:       { name: 'Elena Rostova', title: 'Senior HR Specialist', dept: 'Human Resources', location: 'Bangalore, India' },
  [ROLES.ADMIN]:    { name: 'Marcus Vance', title: 'System Administrator', dept: 'Platform Operations', location: 'Global' },
};

export default function HomeDashboard({ onNavigate }) {
  const { activeRole } = useRole();
  const persona = PERSONA_INFO[activeRole] || PERSONA_INFO[ROLES.EMPLOYEE];
  const now = new Date();
  const greeting = now.getHours() < 12 ? 'Good morning' : now.getHours() < 17 ? 'Good afternoon' : 'Good evening';

  // ==========================================
  // 1. EMPLOYEE DASHBOARD EXPERIENCE
  // ==========================================
  const renderEmployeeDashboard = () => (
    <div className="space-y-8 animate-fade-in">
      {/* Welcome & Persona */}
      <div className="bg-gradient-to-r from-teal-900/40 via-slate-900 to-slate-900 p-6 rounded-3xl border border-teal-500/20 shadow-2xl backdrop-blur-xl flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
        <div>
          <h1 className="text-2xl md:text-3xl font-bold text-slate-100 flex items-center gap-2">
            {greeting}, {persona.name}! 👋
          </h1>
          <p className="text-slate-400 text-sm mt-1">{persona.title} · <span className="text-teal-400 font-medium">{persona.dept}</span> ({persona.location})</p>
          <p className="text-slate-500 text-xs mt-1">
            {now.toLocaleDateString('en-US', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' })}
          </p>
        </div>
        <button 
          onClick={() => onNavigate('copilot')} 
          className="px-5 py-3 bg-teal-600 hover:bg-teal-500 text-white rounded-2xl font-medium text-sm flex items-center gap-2 shadow-lg hover:shadow-teal-500/25 transition-all cursor-pointer flex-shrink-0"
        >
          <Bot className="w-5 h-5 text-teal-100" />
          Ask Employee Copilot
        </button>
      </div>

      {/* Employee Quick Stats */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        <div className="bg-slate-900/90 border border-white/10 p-5 rounded-2xl flex flex-col justify-between shadow-xl backdrop-blur-md hover:border-teal-500/30 transition-colors">
          <div className="flex justify-between items-center mb-2">
            <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Monthly Attendance</span>
            <Clock className="w-5 h-5 text-teal-400" />
          </div>
          <p className="text-3xl font-extrabold text-white">182.5 <span className="text-sm font-normal text-slate-400">Hours</span></p>
          <p className="text-xs text-teal-400 mt-2 flex items-center gap-1"><CheckCircle className="w-3.5 h-3.5" /> 100% Shift compliance</p>
        </div>

        <div className="bg-slate-900/90 border border-white/10 p-5 rounded-2xl flex flex-col justify-between shadow-xl backdrop-blur-md hover:border-blue-500/30 transition-colors">
          <div className="flex justify-between items-center mb-2">
            <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Leave Available</span>
            <Calendar className="w-5 h-5 text-blue-400" />
          </div>
          <p className="text-3xl font-extrabold text-white">24 <span className="text-sm font-normal text-slate-400">Days</span></p>
          <p className="text-xs text-blue-400 mt-2 flex items-center gap-1">Casual, Sick & Comp-off</p>
        </div>

        <div className="bg-slate-900/90 border border-white/10 p-5 rounded-2xl flex flex-col justify-between shadow-xl backdrop-blur-md hover:border-purple-500/30 transition-colors">
          <div className="flex justify-between items-center mb-2">
            <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Goal Attainment</span>
            <Target className="w-5 h-5 text-purple-400" />
          </div>
          <p className="text-3xl font-extrabold text-white">84%</p>
          <p className="text-xs text-purple-400 mt-2 flex items-center gap-1">3 OKRs On Track</p>
        </div>

        <div className="bg-slate-900/90 border border-white/10 p-5 rounded-2xl flex flex-col justify-between shadow-xl backdrop-blur-md hover:border-amber-500/30 transition-colors">
          <div className="flex justify-between items-center mb-2">
            <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Value Points</span>
            <Star className="w-5 h-5 text-amber-400" />
          </div>
          <p className="text-3xl font-extrabold text-white">1,250</p>
          <p className="text-xs text-amber-400 mt-2 flex items-center gap-1">Top 10% Contributor</p>
        </div>
      </div>

      {/* Employee Dedicated Action Grid */}
      <div>
        <h2 className="text-sm font-bold text-slate-400 uppercase tracking-wider mb-4 px-1">My Personal Workspace</h2>
        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
          {[
            { id: 'attendance',    label: 'My Attendance',   icon: Clock,       color: 'text-teal-400',   bg: 'bg-teal-500/10', sub: 'Clock in/out & history' },
            { id: 'leave',        label: 'My Leave',         icon: Calendar,    color: 'text-blue-400',   bg: 'bg-blue-500/10', sub: 'Balances & applications' },
            { id: 'payroll',      label: 'My Payroll',       icon: DollarSign,  color: 'text-emerald-400',bg: 'bg-emerald-500/10', sub: 'Payslips & tax filings' },
            { id: 'performance',  label: 'My Goals',         icon: Target,      color: 'text-purple-400', bg: 'bg-purple-500/10', sub: 'OKRs & self-reviews' },
            { id: 'training',     label: 'My Training',      icon: BookOpen,    color: 'text-pink-400',   bg: 'bg-pink-500/10', sub: 'Assigned modules & certs' },
            { id: 'documents',    label: 'My Documents',     icon: FileText,    color: 'text-orange-400', bg: 'bg-orange-500/10', sub: 'Identity & compliance docs' },
            { id: 'expenses',     label: 'My Expenses',      icon: Receipt,     color: 'text-cyan-400',   bg: 'bg-cyan-500/10', sub: 'Submit & track claims' },
            { id: 'announcements',label: 'My Announcements', icon: Megaphone,   color: 'text-indigo-400', bg: 'bg-indigo-500/10', sub: 'Company updates & policy' },
            { id: 'recognition',  label: 'My Recognition',   icon: Award,       color: 'text-rose-400',   bg: 'bg-rose-500/10', sub: 'Peer appreciations' },
            { id: 'onboarding',   label: 'My Onboarding',    icon: Layers,      color: 'text-teal-300',   bg: 'bg-teal-300/10', sub: 'Checklists & welcome videos' },
            { id: 'analytics',    label: 'Personal Analytics',icon: BarChart2,  color: 'text-lime-400',   bg: 'bg-lime-500/10', sub: 'Individual attendance trends' },
            { id: 'copilot',      label: 'Employee Copilot',  icon: Bot,        color: 'text-amber-300',  bg: 'bg-amber-500/10', sub: 'Contextual AI assistance' },
          ].map((card) => {
            const Icon = card.icon;
            return (
              <button
                key={card.id}
                onClick={() => onNavigate(card.id)}
                className="bg-slate-900/80 border border-white/10 p-5 rounded-2xl text-left hover:border-white/30 hover:scale-[1.02] active:scale-[0.98] transition-all duration-200 cursor-pointer group shadow-lg flex flex-col justify-between"
              >
                <div>
                  <div className={`w-11 h-11 rounded-2xl ${card.bg} flex items-center justify-center mb-4 group-hover:scale-110 transition-transform`}>
                    <Icon className={`w-6 h-6 ${card.color}`} />
                  </div>
                  <p className="font-bold text-slate-100 text-sm md:text-base">{card.label}</p>
                </div>
                <p className="text-slate-400 text-xs mt-2 line-clamp-2 leading-relaxed">{card.sub}</p>
              </button>
            );
          })}
        </div>
      </div>

      {/* Personal Action Highlights */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div className="bg-slate-900/90 border border-white/10 p-6 rounded-3xl shadow-xl backdrop-blur-md">
          <h3 className="font-bold text-white text-base mb-4 flex items-center gap-2">
            <TrendingUp className="w-5 h-5 text-teal-400" /> Recent Attendance History
          </h3>
          <div className="space-y-3">
            {[
              { date: 'June 23, 2026', in: '08:55 AM', out: '05:35 PM', hours: '8h 40m', status: 'On Time' },
              { date: 'June 22, 2026', in: '08:50 AM', out: '05:40 PM', hours: '8h 50m', status: 'On Time' },
              { date: 'June 21, 2026', in: '09:02 AM', out: '05:30 PM', hours: '8h 28m', status: 'Regular' },
            ].map((row, i) => (
              <div key={i} className="flex justify-between items-center p-3.5 bg-slate-950/50 rounded-2xl border border-white/5 text-sm">
                <div>
                  <p className="font-bold text-slate-200">{row.date}</p>
                  <p className="text-xs text-slate-500">Clock In: {row.in} · Clock Out: {row.out}</p>
                </div>
                <div className="text-right">
                  <p className="font-bold text-teal-400">{row.hours}</p>
                  <span className="text-[10px] bg-teal-500/10 text-teal-400 px-2 py-0.5 rounded-full font-medium">{row.status}</span>
                </div>
              </div>
            ))}
          </div>
        </div>

        <div className="bg-slate-900/90 border border-white/10 p-6 rounded-3xl shadow-xl backdrop-blur-md">
          <h3 className="font-bold text-white text-base mb-4 flex items-center gap-2">
            <BookOpen className="w-5 h-5 text-pink-400" /> Mandatory Training Due
          </h3>
          <div className="space-y-4">
            <div className="p-4 bg-slate-950/50 rounded-2xl border border-white/5 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
              <div>
                <span className="px-2.5 py-1 bg-pink-500/10 text-pink-400 font-bold text-xs rounded-lg uppercase tracking-wider">Compliance</span>
                <h4 className="font-bold text-white text-base mt-2">Global Anti-Bribery & Corruption</h4>
                <p className="text-xs text-slate-400 mt-1">Due in 5 days · Est. 45 mins</p>
              </div>
              <button onClick={() => onNavigate('training')} className="w-full sm:w-auto px-4 py-2.5 bg-pink-600 hover:bg-pink-500 text-white font-medium text-sm rounded-xl transition-colors shadow-lg shadow-pink-500/20">
                Start Course
              </button>
            </div>
            <div className="p-4 bg-slate-950/50 rounded-2xl border border-white/5 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
              <div>
                <span className="px-2.5 py-1 bg-purple-500/10 text-purple-400 font-bold text-xs rounded-lg uppercase tracking-wider">Information Security</span>
                <h4 className="font-bold text-white text-base mt-2">Annual Phishing Awareness 2026</h4>
                <p className="text-xs text-slate-400 mt-1">Due in 12 days · Est. 30 mins</p>
              </div>
              <button onClick={() => onNavigate('training')} className="w-full sm:w-auto px-4 py-2.5 bg-purple-600 hover:bg-purple-500 text-white font-medium text-sm rounded-xl transition-colors shadow-lg shadow-purple-500/20">
                Start Course
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );

  // ==========================================
  // 2. MANAGER DASHBOARD EXPERIENCE
  // ==========================================
  const renderManagerDashboard = () => (
    <div className="space-y-8 animate-fade-in">
      {/* Welcome & Persona */}
      <div className="bg-gradient-to-r from-blue-900/40 via-slate-900 to-slate-900 p-6 rounded-3xl border border-blue-500/20 shadow-2xl backdrop-blur-xl flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
        <div>
          <h1 className="text-2xl md:text-3xl font-bold text-slate-100 flex items-center gap-2">
            {greeting}, {persona.name}! 👋
          </h1>
          <p className="text-slate-400 text-sm mt-1">{persona.title} · <span className="text-blue-400 font-medium">{persona.dept}</span> ({persona.location})</p>
          <p className="text-slate-500 text-xs mt-1">
            {now.toLocaleDateString('en-US', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' })}
          </p>
        </div>
        <button 
          onClick={() => onNavigate('copilot')} 
          className="px-5 py-3 bg-blue-600 hover:bg-blue-500 text-white rounded-2xl font-medium text-sm flex items-center gap-2 shadow-lg hover:shadow-blue-500/25 transition-all cursor-pointer flex-shrink-0"
        >
          <Bot className="w-5 h-5 text-blue-100" />
          Ask Manager Copilot
        </button>
      </div>

      {/* Manager Quick Stats */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        <div className="bg-slate-900/90 border border-white/10 p-5 rounded-2xl flex flex-col justify-between shadow-xl backdrop-blur-md hover:border-blue-500/30 transition-colors">
          <div className="flex justify-between items-center mb-2">
            <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Pending Leave Approvals</span>
            <Calendar className="w-5 h-5 text-blue-400" />
          </div>
          <p className="text-3xl font-extrabold text-white">4 <span className="text-sm font-normal text-slate-400">Requests</span></p>
          <p className="text-xs text-blue-400 mt-2 flex items-center gap-1"><AlertCircle className="w-3.5 h-3.5" /> Requires immediate review</p>
        </div>

        <div className="bg-slate-900/90 border border-white/10 p-5 rounded-2xl flex flex-col justify-between shadow-xl backdrop-blur-md hover:border-cyan-500/30 transition-colors">
          <div className="flex justify-between items-center mb-2">
            <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Pending Expense Approvals</span>
            <Receipt className="w-5 h-5 text-cyan-400" />
          </div>
          <p className="text-3xl font-extrabold text-white">7 <span className="text-sm font-normal text-slate-400">Claims</span></p>
          <p className="text-xs text-cyan-400 mt-2 flex items-center gap-1">Totaling $4,320.00</p>
        </div>

        <div className="bg-slate-900/90 border border-white/10 p-5 rounded-2xl flex flex-col justify-between shadow-xl backdrop-blur-md hover:border-teal-500/30 transition-colors">
          <div className="flex justify-between items-center mb-2">
            <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Team Attendance Today</span>
            <Users className="w-5 h-5 text-teal-400" />
          </div>
          <p className="text-3xl font-extrabold text-white">12 / 14 <span className="text-sm font-normal text-slate-400">Online</span></p>
          <p className="text-xs text-teal-400 mt-2 flex items-center gap-1">2 Planned Leaves</p>
        </div>

        <div className="bg-slate-900/90 border border-white/10 p-5 rounded-2xl flex flex-col justify-between shadow-xl backdrop-blur-md hover:border-purple-500/30 transition-colors">
          <div className="flex justify-between items-center mb-2">
            <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Team Performance</span>
            <Target className="w-5 h-5 text-purple-400" />
          </div>
          <p className="text-3xl font-extrabold text-white">88% <span className="text-sm font-normal text-slate-400">OKR Avg</span></p>
          <p className="text-xs text-purple-400 mt-2 flex items-center gap-1">Q2 Review Cycle Active</p>
        </div>
      </div>

      {/* Manager Dedicated Action Grid */}
      <div>
        <h2 className="text-sm font-bold text-slate-400 uppercase tracking-wider mb-4 px-1">Manager Workspace & Workflows</h2>
        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
          {[
            { id: 'leave',        label: 'Pending Leave Approvals', icon: Calendar,    color: 'text-blue-400',   bg: 'bg-blue-500/10', sub: 'Action time-off requests' },
            { id: 'expenses',     label: 'Pending Expense Approvals',icon: Receipt,    color: 'text-cyan-400',   bg: 'bg-cyan-500/10', sub: 'Verify receipts & approve' },
            { id: 'attendance',   label: 'Team Attendance',         icon: Clock,       color: 'text-teal-400',   bg: 'bg-teal-500/10', sub: 'Monitor active status' },
            { id: 'team',         label: 'Direct Reports',          icon: Users,       color: 'text-sky-400',    bg: 'bg-sky-500/10', sub: 'View team hierarchy' },
            { id: 'performance',  label: 'Team Performance',        icon: Target,      color: 'text-purple-400', bg: 'bg-purple-500/10', sub: 'Manage OKRs & reviews' },
            { id: 'performance',  label: 'Upcoming Reviews',        icon: Award,       color: 'text-pink-400',   bg: 'bg-pink-500/10', sub: 'Conduct 1-on-1 appraisals' },
            { id: 'analytics',    label: 'Manager Analytics',       icon: BarChart2,   color: 'text-lime-400',   bg: 'bg-lime-500/10', sub: 'Team attendance metrics' },
            { id: 'announcements',label: 'Team Announcements',      icon: Megaphone,   color: 'text-indigo-400', bg: 'bg-indigo-500/10', sub: 'Broadcast to direct reports' },
            { id: 'copilot',      label: 'Manager Copilot',         icon: Bot,         color: 'text-amber-300',  bg: 'bg-amber-500/10', sub: 'AI assistance for managers' },
          ].map((card, i) => {
            const Icon = card.icon;
            return (
              <button
                key={i}
                onClick={() => onNavigate(card.id)}
                className="bg-slate-900/80 border border-white/10 p-5 rounded-2xl text-left hover:border-white/30 hover:scale-[1.02] active:scale-[0.98] transition-all duration-200 cursor-pointer group shadow-lg flex flex-col justify-between"
              >
                <div>
                  <div className={`w-11 h-11 rounded-2xl ${card.bg} flex items-center justify-center mb-4 group-hover:scale-110 transition-transform`}>
                    <Icon className={`w-6 h-6 ${card.color}`} />
                  </div>
                  <p className="font-bold text-slate-100 text-sm md:text-base">{card.label}</p>
                </div>
                <p className="text-slate-400 text-xs mt-2 line-clamp-2 leading-relaxed">{card.sub}</p>
              </button>
            );
          })}
        </div>
      </div>

      {/* Manager Action Queues */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div className="bg-slate-900/90 border border-white/10 p-6 rounded-3xl shadow-xl backdrop-blur-md">
          <div className="flex justify-between items-center mb-4">
            <h3 className="font-bold text-white text-base flex items-center gap-2">
              <Calendar className="w-5 h-5 text-blue-400" /> Pending Leave Approvals
            </h3>
            <button onClick={() => onNavigate('leave')} className="text-xs text-blue-400 hover:underline font-semibold">View All</button>
          </div>
          <div className="space-y-3">
            {[
              { name: 'David Smith', type: 'Casual Leave', dates: 'June 26 - June 28', days: '3 Days', reason: 'Family function' },
              { name: 'Aisha Khan', type: 'Sick Leave', dates: 'June 24 (Today)', days: '1 Day', reason: 'Dental emergency' },
            ].map((req, i) => (
              <div key={i} className="p-4 bg-slate-950/50 rounded-2xl border border-white/5 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
                <div>
                  <div className="flex items-center gap-2">
                    <p className="font-bold text-slate-200 text-sm">{req.name}</p>
                    <span className="px-2 py-0.5 bg-blue-500/10 text-blue-400 text-[10px] font-bold rounded-full">{req.type}</span>
                  </div>
                  <p className="text-xs text-slate-400 mt-1">{req.dates} ({req.days}) · "{req.reason}"</p>
                </div>
                <div className="flex items-center gap-2 w-full sm:w-auto justify-end">
                  <button onClick={() => alert(`Leave request for ${req.name} approved.`)} className="px-3 py-1.5 bg-emerald-600 hover:bg-emerald-500 text-white rounded-xl text-xs font-bold transition-colors flex items-center gap-1">
                    <Check className="w-3.5 h-3.5" /> Approve
                  </button>
                  <button onClick={() => alert(`Leave request for ${req.name} rejected.`)} className="px-3 py-1.5 bg-rose-600/20 hover:bg-rose-600/30 text-rose-400 border border-rose-500/20 rounded-xl text-xs font-bold transition-colors flex items-center gap-1">
                    <X className="w-3.5 h-3.5" /> Reject
                  </button>
                </div>
              </div>
            ))}
          </div>
        </div>

        <div className="bg-slate-900/90 border border-white/10 p-6 rounded-3xl shadow-xl backdrop-blur-md">
          <div className="flex justify-between items-center mb-4">
            <h3 className="font-bold text-white text-base flex items-center gap-2">
              <Receipt className="w-5 h-5 text-cyan-400" /> Pending Expense Approvals
            </h3>
            <button onClick={() => onNavigate('expenses')} className="text-xs text-cyan-400 hover:underline font-semibold">View All</button>
          </div>
          <div className="space-y-3">
            {[
              { name: 'Robert Johnson', cat: 'Client Travel', amount: '$1,240.00', date: 'June 20, 2026', desc: 'Flight & taxi for client workshop' },
              { name: 'Emily Davis', cat: 'Software License', amount: '$450.00', date: 'June 18, 2026', desc: 'Annual design tool subscription' },
            ].map((exp, i) => (
              <div key={i} className="p-4 bg-slate-950/50 rounded-2xl border border-white/5 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
                <div>
                  <div className="flex items-center gap-2">
                    <p className="font-bold text-slate-200 text-sm">{exp.name}</p>
                    <span className="px-2 py-0.5 bg-cyan-500/10 text-cyan-400 text-[10px] font-bold rounded-full">{exp.cat}</span>
                  </div>
                  <p className="text-xs text-slate-400 mt-1">{exp.desc}</p>
                  <p className="text-[10px] text-slate-500 mt-0.5">Submitted: {exp.date}</p>
                </div>
                <div className="flex flex-col sm:items-end w-full sm:w-auto gap-2">
                  <p className="font-extrabold text-cyan-400 text-base">{exp.amount}</p>
                  <div className="flex items-center gap-2 justify-end w-full">
                    <button onClick={() => alert(`Expense claim for ${exp.name} approved.`)} className="px-3 py-1 bg-emerald-600 hover:bg-emerald-500 text-white rounded-xl text-xs font-bold transition-colors">Approve</button>
                    <button onClick={() => alert(`Expense claim for ${exp.name} rejected.`)} className="px-3 py-1 bg-rose-600/20 hover:bg-rose-600/30 text-rose-400 border border-rose-500/20 rounded-xl text-xs font-bold transition-colors">Reject</button>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );

  // ==========================================
  // 3. HR DASHBOARD EXPERIENCE
  // ==========================================
  const renderHRDashboard = () => (
    <div className="space-y-8 animate-fade-in">
      {/* Welcome & Persona */}
      <div className="bg-gradient-to-r from-purple-900/40 via-slate-900 to-slate-900 p-6 rounded-3xl border border-purple-500/20 shadow-2xl backdrop-blur-xl flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
        <div>
          <h1 className="text-2xl md:text-3xl font-bold text-slate-100 flex items-center gap-2">
            {greeting}, {persona.name}! 👋
          </h1>
          <p className="text-slate-400 text-sm mt-1">{persona.title} · <span className="text-purple-400 font-medium">{persona.dept}</span> ({persona.location})</p>
          <p className="text-slate-500 text-xs mt-1">
            {now.toLocaleDateString('en-US', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' })}
          </p>
        </div>
        <button 
          onClick={() => onNavigate('copilot')} 
          className="px-5 py-3 bg-purple-600 hover:bg-purple-500 text-white rounded-2xl font-medium text-sm flex items-center gap-2 shadow-lg hover:shadow-purple-500/25 transition-all cursor-pointer flex-shrink-0"
        >
          <Bot className="w-5 h-5 text-purple-100" />
          Ask HR Copilot
        </button>
      </div>

      {/* HR Quick Stats */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        <div className="bg-slate-900/90 border border-white/10 p-5 rounded-2xl flex flex-col justify-between shadow-xl backdrop-blur-md hover:border-purple-500/30 transition-colors">
          <div className="flex justify-between items-center mb-2">
            <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Recruitment Pipeline</span>
            <Briefcase className="w-5 h-5 text-purple-400" />
          </div>
          <p className="text-3xl font-extrabold text-white">18 <span className="text-sm font-normal text-slate-400">Candidates</span></p>
          <p className="text-xs text-purple-400 mt-2 flex items-center gap-1">5 Interviews Scheduled</p>
        </div>

        <div className="bg-slate-900/90 border border-white/10 p-5 rounded-2xl flex flex-col justify-between shadow-xl backdrop-blur-md hover:border-orange-500/30 transition-colors">
          <div className="flex justify-between items-center mb-2">
            <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Pending Verification</span>
            <FileCheck className="w-5 h-5 text-orange-400" />
          </div>
          <p className="text-3xl font-extrabold text-white">12 <span className="text-sm font-normal text-slate-400">Documents</span></p>
          <p className="text-xs text-orange-400 mt-2 flex items-center gap-1">Identity & tax proofs</p>
        </div>

        <div className="bg-slate-900/90 border border-white/10 p-5 rounded-2xl flex flex-col justify-between shadow-xl backdrop-blur-md hover:border-teal-500/30 transition-colors">
          <div className="flex justify-between items-center mb-2">
            <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Active Onboarding</span>
            <Layers className="w-5 h-5 text-teal-400" />
          </div>
          <p className="text-3xl font-extrabold text-white">6 <span className="text-sm font-normal text-slate-400">New Joiners</span></p>
          <p className="text-xs text-teal-400 mt-2 flex items-center gap-1">Avg progress 62%</p>
        </div>

        <div className="bg-slate-900/90 border border-white/10 p-5 rounded-2xl flex flex-col justify-between shadow-xl backdrop-blur-md hover:border-emerald-500/30 transition-colors">
          <div className="flex justify-between items-center mb-2">
            <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Compliance Status</span>
            <DollarSign className="w-5 h-5 text-emerald-400" />
          </div>
          <p className="text-3xl font-extrabold text-white">100%</p>
          <p className="text-xs text-emerald-400 mt-2 flex items-center gap-1"><CheckCircle className="w-3.5 h-3.5" /> US & India filings ready</p>
        </div>
      </div>

      {/* HR Dedicated Action Grid */}
      <div>
        <h2 className="text-sm font-bold text-slate-400 uppercase tracking-wider mb-4 px-1">HR Workspace & Workflows</h2>
        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
          {[
            { id: 'recruitment',  label: 'Recruitment',       icon: Briefcase,   color: 'text-purple-400', bg: 'bg-purple-500/10', sub: 'Manage job postings' },
            { id: 'recruitment',  label: 'Candidates',        icon: UserCheck,   color: 'text-pink-400',   bg: 'bg-pink-500/10', sub: 'Track interview pipeline' },
            { id: 'payroll',      label: 'Payroll Processing',icon: DollarSign,  color: 'text-emerald-400',bg: 'bg-emerald-500/10', sub: 'Generate payslips & tax' },
            { id: 'documents',    label: 'Document Verification',icon: FileCheck, color: 'text-orange-400', bg: 'bg-orange-500/10', sub: 'Verify KYC & employment' },
            { id: 'onboarding',   label: 'Onboarding Queue',  icon: Layers,      color: 'text-teal-400',   bg: 'bg-teal-500/10', sub: 'Monitor new joiners' },
            { id: 'training',     label: 'Training Assignment',icon: BookOpen,   color: 'text-sky-400',    bg: 'bg-sky-500/10', sub: 'Assign mandatory courses' },
            { id: 'payroll',      label: 'Compliance',        icon: Shield,      color: 'text-blue-400',   bg: 'bg-blue-500/10', sub: 'Statutory compliance tracking' },
            { id: 'announcements',label: 'Announcements',     icon: Megaphone,   color: 'text-indigo-400', bg: 'bg-indigo-500/10', sub: 'Create company broadcasts' },
            { id: 'analytics',    label: 'HR Analytics',      icon: BarChart2,   color: 'text-lime-400',   bg: 'bg-lime-500/10', sub: 'Org-wide workforce metrics' },
            { id: 'team',         label: 'Employee Directory',icon: Users,       color: 'text-rose-400',   bg: 'bg-rose-500/10', sub: 'All employee search & profiles' },
            { id: 'copilot',      label: 'HR Copilot',        icon: Bot,         color: 'text-amber-300',  bg: 'bg-amber-500/10', sub: 'AI assistant for HR specialists' },
          ].map((card, i) => {
            const Icon = card.icon;
            return (
              <button
                key={i}
                onClick={() => onNavigate(card.id)}
                className="bg-slate-900/80 border border-white/10 p-5 rounded-2xl text-left hover:border-white/30 hover:scale-[1.02] active:scale-[0.98] transition-all duration-200 cursor-pointer group shadow-lg flex flex-col justify-between"
              >
                <div>
                  <div className={`w-11 h-11 rounded-2xl ${card.bg} flex items-center justify-center mb-4 group-hover:scale-110 transition-transform`}>
                    <Icon className={`w-6 h-6 ${card.color}`} />
                  </div>
                  <p className="font-bold text-slate-100 text-sm md:text-base">{card.label}</p>
                </div>
                <p className="text-slate-400 text-xs mt-2 line-clamp-2 leading-relaxed">{card.sub}</p>
              </button>
            );
          })}
        </div>
      </div>

      {/* HR Action Verification Panels */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div className="bg-slate-900/90 border border-white/10 p-6 rounded-3xl shadow-xl backdrop-blur-md">
          <div className="flex justify-between items-center mb-4">
            <h3 className="font-bold text-white text-base flex items-center gap-2">
              <UserCheck className="w-5 h-5 text-purple-400" /> Active Candidate Pipeline
            </h3>
            <button onClick={() => onNavigate('recruitment')} className="text-xs text-purple-400 hover:underline font-semibold">View ATS</button>
          </div>
          <div className="space-y-3">
            {[
              { name: 'Arjun Mehta', role: 'Staff Backend Engineer', stage: 'Technical Interview', status: 'In Progress', rating: '4.8 ★' },
              { name: 'Chloe Grace', role: 'Product Designer', stage: 'HR Screening', status: 'Scheduled', rating: '4.5 ★' },
            ].map((cand, i) => (
              <div key={i} className="p-4 bg-slate-950/50 rounded-2xl border border-white/5 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
                <div>
                  <div className="flex items-center gap-2">
                    <p className="font-bold text-slate-200 text-sm">{cand.name}</p>
                    <span className="px-2 py-0.5 bg-purple-500/10 text-purple-400 text-[10px] font-bold rounded-full">{cand.rating}</span>
                  </div>
                  <p className="text-xs text-slate-400 mt-1">Applied: {cand.role} · Stage: <strong className="text-slate-200">{cand.stage}</strong></p>
                </div>
                <button onClick={() => onNavigate('recruitment')} className="px-4 py-2 bg-purple-600 hover:bg-purple-500 text-white rounded-xl text-xs font-bold transition-colors w-full sm:w-auto">
                  Schedule Next
                </button>
              </div>
            ))}
          </div>
        </div>

        <div className="bg-slate-900/90 border border-white/10 p-6 rounded-3xl shadow-xl backdrop-blur-md">
          <div className="flex justify-between items-center mb-4">
            <h3 className="font-bold text-white text-base flex items-center gap-2">
              <FileCheck className="w-5 h-5 text-orange-400" /> Document Verification Queue
            </h3>
            <button onClick={() => onNavigate('documents')} className="text-xs text-orange-400 hover:underline font-semibold">Verify All</button>
          </div>
          <div className="space-y-3">
            {[
              { name: 'Alex joiner', doc: 'Work Authorization / Visa Proof', type: 'PDF', date: 'Submitted 2 hours ago' },
              { name: 'Sanjay Kumar', doc: 'Previous Relieving Letter & Tax Form', type: 'PDF', date: 'Submitted 1 day ago' },
            ].map((doc, i) => (
              <div key={i} className="p-4 bg-slate-950/50 rounded-2xl border border-white/5 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
                <div>
                  <div className="flex items-center gap-2">
                    <p className="font-bold text-slate-200 text-sm">{doc.name}</p>
                    <span className="px-2 py-0.5 bg-orange-500/10 text-orange-400 text-[10px] font-bold rounded-full">{doc.type}</span>
                  </div>
                  <p className="text-xs text-slate-400 mt-1">{doc.doc}</p>
                  <p className="text-[10px] text-slate-500 mt-0.5">{doc.date}</p>
                </div>
                <div className="flex items-center gap-2 w-full sm:w-auto justify-end">
                  <button onClick={() => alert(`Document for ${doc.name} verified.`)} className="px-3 py-1.5 bg-emerald-600 hover:bg-emerald-500 text-white rounded-xl text-xs font-bold transition-colors">Verify</button>
                  <button onClick={() => alert(`Document for ${doc.name} rejected.`)} className="px-3 py-1.5 bg-rose-600/20 hover:bg-rose-600/30 text-rose-400 border border-rose-500/20 rounded-xl text-xs font-bold transition-colors">Reject</button>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );

  // ==========================================
  // 4. ADMIN DASHBOARD EXPERIENCE
  // ==========================================
  const renderAdminDashboard = () => (
    <div className="space-y-8 animate-fade-in">
      {/* Welcome & Persona */}
      <div className="bg-gradient-to-r from-amber-900/40 via-slate-900 to-slate-900 p-6 rounded-3xl border border-amber-500/20 shadow-2xl backdrop-blur-xl flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
        <div>
          <h1 className="text-2xl md:text-3xl font-bold text-slate-100 flex items-center gap-2">
            {greeting}, {persona.name}! 👋
          </h1>
          <p className="text-slate-400 text-sm mt-1">{persona.title} · <span className="text-amber-400 font-medium">{persona.dept}</span> ({persona.location})</p>
          <p className="text-slate-500 text-xs mt-1">
            {now.toLocaleDateString('en-US', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' })}
          </p>
        </div>
        <button 
          onClick={() => onNavigate('copilot')} 
          className="px-5 py-3 bg-amber-600 hover:bg-amber-500 text-white rounded-2xl font-medium text-sm flex items-center gap-2 shadow-lg hover:shadow-amber-500/25 transition-all cursor-pointer flex-shrink-0"
        >
          <Bot className="w-5 h-5 text-amber-100" />
          Ask Admin Copilot
        </button>
      </div>

      {/* Admin Quick Stats */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        <div className="bg-slate-900/90 border border-white/10 p-5 rounded-2xl flex flex-col justify-between shadow-xl backdrop-blur-md hover:border-amber-500/30 transition-colors">
          <div className="flex justify-between items-center mb-2">
            <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Organization Overview</span>
            <Users className="w-5 h-5 text-amber-400" />
          </div>
          <p className="text-3xl font-extrabold text-white">1,428 <span className="text-sm font-normal text-slate-400">Total Users</span></p>
          <p className="text-xs text-amber-400 mt-2 flex items-center gap-1">4 Dynamic Roles Enforced</p>
        </div>

        <div className="bg-slate-900/90 border border-white/10 p-5 rounded-2xl flex flex-col justify-between shadow-xl backdrop-blur-md hover:border-emerald-500/30 transition-colors">
          <div className="flex justify-between items-center mb-2">
            <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Platform Health</span>
            <Activity className="w-5 h-5 text-emerald-400" />
          </div>
          <p className="text-3xl font-extrabold text-white">99.99%</p>
          <p className="text-xs text-emerald-400 mt-2 flex items-center gap-1"><CheckCircle className="w-3.5 h-3.5" /> All Microservices Operational</p>
        </div>

        <div className="bg-slate-900/90 border border-white/10 p-5 rounded-2xl flex flex-col justify-between shadow-xl backdrop-blur-md hover:border-blue-500/30 transition-colors">
          <div className="flex justify-between items-center mb-2">
            <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Database Integrity</span>
            <Database className="w-5 h-5 text-blue-400" />
          </div>
          <p className="text-3xl font-extrabold text-white">0 <span className="text-sm font-normal text-slate-400">Warnings</span></p>
          <p className="text-xs text-blue-400 mt-2 flex items-center gap-1">EF Core Postgres Synchronized</p>
        </div>

        <div className="bg-slate-900/90 border border-white/10 p-5 rounded-2xl flex flex-col justify-between shadow-xl backdrop-blur-md hover:border-purple-500/30 transition-colors">
          <div className="flex justify-between items-center mb-2">
            <span className="text-xs font-semibold text-slate-400 uppercase tracking-wider">System Statistics</span>
            <Key className="w-5 h-5 text-purple-400" />
          </div>
          <p className="text-3xl font-extrabold text-white">2,845 <span className="text-sm font-normal text-slate-400">Auth Check/hr</span></p>
          <p className="text-xs text-purple-400 mt-2 flex items-center gap-1">0 Unauthorized breaches</p>
        </div>
      </div>

      {/* Admin Dedicated Action Grid */}
      <div>
        <h2 className="text-sm font-bold text-slate-400 uppercase tracking-wider mb-4 px-1">Admin Workspace & Permissions</h2>
        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
          {[
            { id: 'team',         label: 'Organization Overview',icon: Users,       color: 'text-amber-400',  bg: 'bg-amber-500/10', sub: 'System-wide user catalog' },
            { id: 'analytics',    label: 'System Statistics',    icon: Activity,    color: 'text-emerald-400',bg: 'bg-emerald-500/10', sub: 'Real-time performance graphs' },
            { id: 'team',         label: 'User Management',      icon: UserCheck,   color: 'text-blue-400',   bg: 'bg-blue-500/10', sub: 'Add, update & disable accounts' },
            { id: 'team',         label: 'Role Management',      icon: Key,         color: 'text-purple-400', bg: 'bg-purple-500/10', sub: 'Configure RBAC matrices' },
            { id: 'team',         label: 'Security',             icon: Shield,      color: 'text-rose-400',   bg: 'bg-rose-500/10', sub: 'JWT authorization policies' },
            { id: 'home',         label: 'Configuration',        icon: Settings,    color: 'text-cyan-400',   bg: 'bg-cyan-500/10', sub: 'Global HRMS environment setup' },
            { id: 'home',         label: 'Audit Logs',           icon: FileText,    color: 'text-orange-400', bg: 'bg-orange-500/10', sub: 'Immutable system access logs' },
            { id: 'analytics',    label: 'System Analytics',     icon: BarChart2,   color: 'text-lime-400',   bg: 'bg-lime-500/10', sub: 'Server & GraphQL telemetry' },
            { id: 'home',         label: 'Platform Health',      icon: Database,    color: 'text-teal-400',   bg: 'bg-teal-500/10', sub: 'Inspect Postgres EF Core health' },
            { id: 'analytics',    label: 'Organization Metrics', icon: TrendingUp,  color: 'text-pink-400',   bg: 'bg-pink-500/10', sub: 'Aggregated company KPIs' },
            { id: 'copilot',      label: 'Admin Copilot',        icon: Bot,         color: 'text-amber-300',  bg: 'bg-amber-500/10', sub: 'AI assistant for platform admins' },
          ].map((card, i) => {
            const Icon = card.icon;
            return (
              <button
                key={i}
                onClick={() => onNavigate(card.id)}
                className="bg-slate-900/80 border border-white/10 p-5 rounded-2xl text-left hover:border-white/30 hover:scale-[1.02] active:scale-[0.98] transition-all duration-200 cursor-pointer group shadow-lg flex flex-col justify-between"
              >
                <div>
                  <div className={`w-11 h-11 rounded-2xl ${card.bg} flex items-center justify-center mb-4 group-hover:scale-110 transition-transform`}>
                    <Icon className={`w-6 h-6 ${card.color}`} />
                  </div>
                  <p className="font-bold text-slate-100 text-sm md:text-base">{card.label}</p>
                </div>
                <p className="text-slate-400 text-xs mt-2 line-clamp-2 leading-relaxed">{card.sub}</p>
              </button>
            );
          })}
        </div>
      </div>

      {/* Admin Security & Audit Log Table */}
      <div className="bg-slate-900/90 border border-white/10 p-6 rounded-3xl shadow-xl backdrop-blur-md">
        <div className="flex justify-between items-center mb-4">
          <h3 className="font-bold text-white text-base flex items-center gap-2">
            <Shield className="w-5 h-5 text-amber-400" /> Real-time System Security & Audit Logs
          </h3>
          <span className="text-xs bg-emerald-500/10 text-emerald-400 px-3 py-1 rounded-full font-bold border border-emerald-500/20">Active Enforcement</span>
        </div>
        <div className="overflow-x-auto">
          <table className="w-full text-left text-sm text-slate-300 border-collapse">
            <thead>
              <tr className="border-b border-white/10 text-slate-400 text-xs uppercase tracking-wider">
                <th className="py-3 px-4">Timestamp</th>
                <th className="py-3 px-4">Action</th>
                <th className="py-3 px-4">Subject Role</th>
                <th className="py-3 px-4">Target Endpoint</th>
                <th className="py-3 px-4">Verification State</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-white/5">
              {[
                { time: '2026-06-24 13:41:02', action: 'EF Core DB Initialization', role: 'System Admin', target: 'PostgresDbContext', status: 'PASS - No Warnings' },
                { time: '2026-06-24 13:40:55', action: 'GraphQL Query Execution', role: 'HR Specialist', target: 'GetCandidates()', status: 'PASS - Valid JWT' },
                { time: '2026-06-24 13:40:12', action: 'Role Capability Verification', role: 'Manager', target: 'ApproveLeave()', status: 'PASS - Authorized' },
                { time: '2026-06-24 13:39:48', action: 'Direct URL Navigation', role: 'Employee', target: '/recruitment (Prohibited)', status: 'PASS - Clean Reject' },
              ].map((log, i) => (
                <tr key={i} className="hover:bg-slate-950/40 transition-colors">
                  <td className="py-3.5 px-4 font-mono text-xs text-slate-400">{log.time}</td>
                  <td className="py-3.5 px-4 font-semibold text-white">{log.action}</td>
                  <td className="py-3.5 px-4 text-amber-400 font-medium">{log.role}</td>
                  <td className="py-3.5 px-4 font-mono text-xs text-slate-300">{log.target}</td>
                  <td className="py-3.5 px-4">
                    <span className="px-2.5 py-1 bg-emerald-500/10 text-emerald-400 text-[11px] font-bold rounded-full border border-emerald-500/20">
                      {log.status}
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );

  switch (activeRole) {
    case ROLES.MANAGER:            return renderManagerDashboard();
    case ROLES.REPORTING_MANAGER:  return renderManagerDashboard();
    case ROLES.HR:                 return renderHRDashboard();
    case ROLES.ADMIN:              return renderAdminDashboard();
    case ROLES.EMPLOYEE:
    default:                       return renderEmployeeDashboard();
  }
}
