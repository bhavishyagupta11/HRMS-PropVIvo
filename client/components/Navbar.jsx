import React from 'react';

export default function Navbar({ activeTab, setActiveTab }) {
  const tabs = [
    { id: 'documents', label: 'Documents' },
    { id: 'expenses', label: 'Expenses' },
    { id: 'performance', label: 'Performance & Goals' },
    { id: 'training', label: 'Training & Certs' },
    { id: 'recruitment', label: 'Recruitment & ATS' },
    { id: 'recognition', label: 'Peer Recognition' },
    { id: 'announcements', label: 'Announcements' },
    { id: 'analytics', label: 'HR Analytics' },
    { id: 'copilot', label: 'HR Copilot AI' },
    { id: 'attendance', label: 'Attendance' },
    { id: 'leave', label: 'Leave Management' },
    { id: 'payroll', label: 'Payroll & Compliance' },
    { id: 'onboarding', label: 'Onboarding' },
    { id: 'identity', label: 'Identity & Auth' },
  ];

  return (
    <nav className="navbar">
      <div className="navbar-brand">
        <svg width="32" height="32" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
          <path d="M12 2L2 7L12 12L22 7L12 2Z" stroke="#6366f1" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
          <path d="M2 17L12 22L22 17" stroke="#a855f7" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
          <path d="M2 12L12 17L22 12" stroke="#6366f1" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
        </svg>
        HRMS Enterprise Dashboard
      </div>
      <div className="navbar-nav">
        {tabs.map((tab) => (
          <button
            key={tab.id}
            className={`nav-btn ${activeTab === tab.id ? 'active' : ''}`}
            onClick={() => setActiveTab(tab.id)}
          >
            {tab.label}
          </button>
        ))}
      </div>
    </nav>
  );
}
