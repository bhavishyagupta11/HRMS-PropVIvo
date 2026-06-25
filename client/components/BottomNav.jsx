'use client';

/**
 * BottomNav — PSD Section 3.2: Role-Specific Bottom Navigation
 * Employee: Home, Attendance, Performance, Training, Contributions
 * Manager:  Home, Team, Leave, Performance, Training
 * HR:       Home, Recruitment, Analytics, Training, Announcements
 * Admin:    Home, Analytics, Team, Training, Announcements
 */

import React from 'react';
import {
  Home, Clock, Target, BookOpen, Star,
  Users, Calendar, Briefcase, BarChart2, Megaphone
} from 'lucide-react';
import { useRole } from '../context/RoleContext';

const ICON_MAP = {
  Home, Clock, Target, BookOpen, Star,
  Users, Calendar, Briefcase, BarChart2, Megaphone
};

export default function BottomNav({ activeTab, onTabChange }) {
  const { navItems } = useRole();

  return (
    <nav className="bottom-nav bg-slate-900 border-t border-white/10 py-2 px-3 flex items-center justify-around z-40 flex-shrink-0 shadow-2xl backdrop-blur-2xl md:hidden">
      {navItems.map((item) => {
        const Icon = ICON_MAP[item.icon];
        const isActive = activeTab === item.id;
        return (
          <button
            key={item.id}
            onClick={() => onTabChange(item.id)}
            className={`bottom-nav-item flex flex-col items-center gap-1 p-2 rounded-2xl transition-all duration-150 cursor-pointer flex-1 text-center
              ${isActive ? 'active bg-teal-500/10 text-teal-400 font-bold' : 'text-slate-400 hover:text-slate-200 hover:bg-white/5'}`}
            aria-label={item.label}
            aria-current={isActive ? 'page' : undefined}
            tabIndex={0}
          >
            {Icon && (
              <Icon
                className={`w-5 h-5 transition-transform ${isActive ? 'scale-110 text-teal-400' : 'text-slate-400'}`}
                strokeWidth={isActive ? 2.5 : 2}
              />
            )}
            <span className="text-[10px] font-semibold tracking-wide truncate block w-full">{item.label}</span>
          </button>
        );
      })}
    </nav>
  );
}
