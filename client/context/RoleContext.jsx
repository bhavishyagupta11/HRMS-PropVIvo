'use client';

/**
 * RoleContext — PSD Section 3: Role-Based Access Control
 * Provides the active role and role switcher across the app.
 * Roles: Employee, Manager, HR, Admin (PSD Section 2.2 Personas)
 */

import React, { createContext, useContext, useState, useEffect } from 'react';

export const ROLES = {
  EMPLOYEE: 'Employee',
  MANAGER: 'Manager',
  REPORTING_MANAGER: 'Reporting Manager',
  HR: 'HR',
  ADMIN: 'Admin',
};

// PSD Section 3.1 Role Capability Matrix
export const ROLE_CAPABILITIES = {
  [ROLES.EMPLOYEE]: {
    canApproveLeave: false,
    canApproveExpense: false,
    canViewRecruitment: false,
    canCreateAnnouncement: false,
    canViewTeam: false,
    analyticsScope: 'self',
    canAccessAdmin: false,
  },
  [ROLES.MANAGER]: {
    canApproveLeave: true,
    canApproveExpense: true,
    canViewRecruitment: false,
    canCreateAnnouncement: false,
    canViewTeam: true,
    analyticsScope: 'team',
    canAccessAdmin: false,
  },
  [ROLES.REPORTING_MANAGER]: {
    canApproveLeave: true,
    canApproveExpense: true,
    canViewRecruitment: false,
    canCreateAnnouncement: false,
    canViewTeam: true,
    analyticsScope: 'team',
    canAccessAdmin: false,
  },
  [ROLES.HR]: {
    canApproveLeave: true,
    canApproveExpense: true,
    canViewRecruitment: true,
    canCreateAnnouncement: true,
    canViewTeam: true,
    analyticsScope: 'org',
    canAccessAdmin: false,
  },
  [ROLES.ADMIN]: {
    canApproveLeave: true,
    canApproveExpense: true,
    canViewRecruitment: true,
    canCreateAnnouncement: true,
    canViewTeam: true,
    analyticsScope: 'org',
    canAccessAdmin: true,
  },
};

// PSD Section 3.2: Role-Specific Bottom Navigation
export const ROLE_NAV = {
  [ROLES.EMPLOYEE]: [
    { id: 'home', label: 'Home', icon: 'Home' },
    { id: 'attendance', label: 'Attendance', icon: 'Clock' },
    { id: 'performance', label: 'Performance', icon: 'Target' },
    { id: 'training', label: 'Training', icon: 'BookOpen' },
    { id: 'contributions', label: 'Contributions', icon: 'Star' },
  ],
  [ROLES.MANAGER]: [
    { id: 'home', label: 'Home', icon: 'Home' },
    { id: 'team', label: 'Team', icon: 'Users' },
    { id: 'leave', label: 'Leave', icon: 'Calendar' },
    { id: 'performance', label: 'Performance', icon: 'Target' },
    { id: 'training', label: 'Training', icon: 'BookOpen' },
  ],
  [ROLES.REPORTING_MANAGER]: [
    { id: 'home', label: 'Home', icon: 'Home' },
    { id: 'team', label: 'Team', icon: 'Users' },
    { id: 'leave', label: 'Leave', icon: 'Calendar' },
    { id: 'performance', label: 'Performance', icon: 'Target' },
    { id: 'training', label: 'Training', icon: 'BookOpen' },
  ],
  [ROLES.HR]: [
    { id: 'home', label: 'Home', icon: 'Home' },
    { id: 'recruitment', label: 'Recruitment', icon: 'Briefcase' },
    { id: 'analytics', label: 'Analytics', icon: 'BarChart2' },
    { id: 'training', label: 'Training', icon: 'BookOpen' },
    { id: 'announcements', label: 'Announce', icon: 'Megaphone' },
  ],
  [ROLES.ADMIN]: [
    { id: 'home', label: 'Home', icon: 'Home' },
    { id: 'analytics', label: 'Analytics', icon: 'BarChart2' },
    { id: 'team', label: 'Team', icon: 'Users' },
    { id: 'training', label: 'Training', icon: 'BookOpen' },
    { id: 'announcements', label: 'Announce', icon: 'Megaphone' },
  ],
};

const RoleContext = createContext(null);

export function RoleProvider({ children }) {
  const [activeRole, setActiveRole] = useState(ROLES.EMPLOYEE);
  const [token, setToken] = useState(null);

  const capabilities = ROLE_CAPABILITIES[activeRole];
  const navItems = ROLE_NAV[activeRole];

  // GraphQL Client helper
  const graphqlFetch = async (query, variables = {}, customToken = null) => {
    const headers = {
      'Content-Type': 'application/json',
    };
    const activeToken = customToken || token || sessionStorage.getItem('hrms_token');
    if (activeToken && activeToken !== 'ANONYMOUS') {
      headers['Authorization'] = `Bearer ${activeToken}`;
    }
    const res = await fetch('http://localhost:5056/graphql', {
      method: 'POST',
      headers,
      body: JSON.stringify({ query, variables }),
    });
    const data = await res.json();
    if (data.errors) {
      throw new Error(data.errors[0]?.message || 'GraphQL API Request Failed');
    }
    return data.data;
  };

  // Automatically login backend account when switching roles
  useEffect(() => {
    const loginAndSync = async () => {
      let email = 'employee@hrms.com';
      if (activeRole === ROLES.MANAGER) email = 'manager@hrms.com';
      else if (activeRole === ROLES.REPORTING_MANAGER) email = 'repmanager@hrms.com';
      else if (activeRole === ROLES.HR) email = 'hr@hrms.com';
      else if (activeRole === ROLES.ADMIN) email = 'admin@hrms.com';

      try {
        const query = `
          mutation Login($request: LoginRequestInput!) {
            login(request: $request) {
              token
            }
          }
        `;
        const password = activeRole === ROLES.ADMIN ? 'Admin@123' : 'Role@123';
        const variables = { request: { email, password } };
        const response = await graphqlFetch(query, variables, 'ANONYMOUS');
        if (response && response.login && response.login.token) {
          setToken(response.login.token);
          sessionStorage.setItem('hrms_token', response.login.token);
        }
      } catch (err) {
        console.error('Auto login failed for role:', activeRole, err);
      }
    };

    loginAndSync();
  }, [activeRole]);

  // Read saved token on mount
  useEffect(() => {
    const savedToken = sessionStorage.getItem('hrms_token');
    if (savedToken) {
      setToken(savedToken);
    }
  }, []);

  return (
    <RoleContext.Provider value={{ activeRole, setActiveRole, capabilities, navItems, ROLES, token, setToken, graphqlFetch }}>
      {children}
    </RoleContext.Provider>
  );
}

export function useRole() {
  const ctx = useContext(RoleContext);
  if (!ctx) throw new Error('useRole must be used within RoleProvider');
  return ctx;
}
