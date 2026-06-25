import React from 'react';
import { RoleProvider } from '../context/RoleContext';
import './globals.css';

export const metadata = {
  title: 'WorkFlow · Global HRMS Platform',
  description: 'Enterprise HRMS platform featuring role-based access control, automated attendance, localized payroll, and embedded AI HR Copilot.',
};

export default function RootLayout({ children }) {
  return (
    <html lang="en">
      <body>
        <RoleProvider>
          {children}
        </RoleProvider>
      </body>
    </html>
  );
}
