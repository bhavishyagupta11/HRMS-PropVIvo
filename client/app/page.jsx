'use client';

import React, { useState } from 'react';
import Header from '../components/Header';
import BottomNav from '../components/BottomNav';
import Sidebar from '../components/Sidebar';
import HomeDashboard from '../components/HomeDashboard';
import OnboardingFeature from '../components/OnboardingFeature';
import AttendanceFeature from '../components/AttendanceFeature';
import LeaveFeature from '../components/LeaveFeature';
import PayrollFeature from '../components/PayrollFeature';
import DocumentFeature from '../components/DocumentFeature';
import ExpenseFeature from '../components/ExpenseFeature';
import PerformanceFeature from '../components/PerformanceFeature';
import ContributionsFeature from '../components/ContributionsFeature';
import TrainingFeature from '../components/TrainingFeature';
import RecruitmentFeature from '../components/RecruitmentFeature';
import RecognitionFeature from '../components/RecognitionFeature';
import AnnouncementFeature from '../components/AnnouncementFeature';
import TeamFeature from '../components/TeamFeature';
import AnalyticsFeature from '../components/AnalyticsFeature';
import CopilotFeature from '../components/CopilotFeature';
import { useRole } from '../context/RoleContext';

export default function Page() {
  const { activeRole } = useRole();
  const [activeTab, setActiveTab] = useState('home');
  const [showCopilotModal, setShowCopilotModal] = useState(false);

  // Render main module content based on activeTab
  const renderContent = () => {
    switch (activeTab) {
      case 'home':          return <HomeDashboard onNavigate={(tab) => setActiveTab(tab)} />;
      case 'onboarding':    return <OnboardingFeature onComplete={() => setActiveTab('home')} />;
      case 'attendance':    return <AttendanceFeature />;
      case 'leave':         return <LeaveFeature />;
      case 'payroll':       return <PayrollFeature />;
      case 'documents':     return <DocumentFeature />;
      case 'expenses':      return <ExpenseFeature />;
      case 'performance':   return <PerformanceFeature />;
      case 'contributions': return <ContributionsFeature />;
      case 'training':      return <TrainingFeature />;
      case 'recruitment':   return <RecruitmentFeature />;
      case 'recognition':   return <RecognitionFeature />;
      case 'announcements': return <AnnouncementFeature />;
      case 'team':          return <TeamFeature />;
      case 'analytics':     return <AnalyticsFeature />;
      case 'copilot':       return <CopilotFeature currentView={activeTab} isOnboarding={activeTab === 'onboarding'} />;
      default:              return <HomeDashboard onNavigate={(tab) => setActiveTab(tab)} />;
    }
  };

  return (
    <div className="app-layout">
      {/* Desktop Sidebar — PSD Section 3.2 */}
      <Sidebar activeTab={activeTab} onTabChange={(tab) => setActiveTab(tab)} />

      {/* Main Panel */}
      <div className="main-panel flex flex-col min-w-0">
        {/* Header with Role Switcher, Notifications & Copilot Quick Access — PSD Section 3, 4 & 4.15 */}
        <Header onCopilotOpen={() => setShowCopilotModal(true)} onNavigate={(tab) => setActiveTab(tab)} />

        {/* Dynamic Page Content */}
        <main className="page-content flex-1 overflow-y-auto p-4 md:p-6 lg:p-8 min-w-0">
          {renderContent()}
        </main>

        {/* Mobile Bottom Navigation — PSD Section 3.2 */}
        <BottomNav activeTab={activeTab} onTabChange={(tab) => setActiveTab(tab)} />
      </div>

      {/* Embedded HR Copilot AI Modal Panel — PSD Section 4.15 (reachable from any view) */}
      {showCopilotModal && (
        <div className="modal-backdrop z-50 fixed inset-0 bg-slate-950/80 backdrop-blur-sm flex items-center justify-center p-4">
          <CopilotFeature
            currentView={activeTab}
            isOnboarding={activeTab === 'onboarding'}
            asPanel={true}
            onClose={() => setShowCopilotModal(false)}
          />
        </div>
      )}
    </div>
  );
}
