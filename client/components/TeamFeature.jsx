'use client';

import React, { useState } from 'react';
import { Users, Search, Mail, MapPin, Calendar, ExternalLink, ShieldAlert } from 'lucide-react';
import { useRole, ROLES } from '../context/RoleContext';

export default function TeamFeature() {
  const { activeRole, capabilities, graphqlFetch, token } = useRole();
  const [searchQuery, setSearchQuery] = useState('');
  const [deptFilter, setDeptFilter] = useState('all');
  const [selectedEmp, setSelectedEmp] = useState(null);
  
  const [teamMembers, setTeamMembers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  React.useEffect(() => {
    if (!token || !capabilities.canViewTeam) return;

    const loadTeam = async () => {
      setLoading(true);
      setError(null);
      try {
        const query = `
          query {
            teamMembers {
              id name avatar designation department status manager reportingManager email joinDate location color
            }
          }
        `;
        const data = await graphqlFetch(query);
        if (data && data.teamMembers) {
          setTeamMembers(data.teamMembers);
        }
      } catch (err) {
        setError(err.message || 'Failed to fetch team members');
      } finally {
        setLoading(false);
      }
    };
    loadTeam();
  }, [token, capabilities.canViewTeam]);

  // PSD Section 3.1: Access restriction
  if (!capabilities.canViewTeam) {
    return (
      <div className="glass-card empty-state">
        <div className="empty-icon"><ShieldAlert className="w-8 h-8 text-red-500" /></div>
        <h3 className="empty-title">Manager Access Required</h3>
        <p className="empty-desc">The Team Roster & Roster Management view is restricted to Manager, HR, and Admin roles per PSD RBAC specifications.</p>
      </div>
    );
  }

  const filteredTeam = teamMembers.filter(m => {
    const matchesSearch = m.name.toLowerCase().includes(searchQuery.toLowerCase()) || m.designation.toLowerCase().includes(searchQuery.toLowerCase());
    const matchesDept = deptFilter === 'all' || m.department.toLowerCase() === deptFilter.toLowerCase();
    return matchesSearch && matchesDept;
  });

  return (
    <div className="animate-fade-in space-y-6">
      <div className="section-header">
        <div>
          <h1 className="section-title"><Users className="w-7 h-7 text-teal-500" /> Team Roster & Hierarchy</h1>
          <p className="section-subtitle">Organizational hierarchy, reporting managers, and direct report management</p>
        </div>
      </div>

      {loading && <div className="text-center py-8 text-slate-400">Loading team roster...</div>}
      {error && <div className="text-center py-8 text-amber-500">{error}</div>}

      {!loading && !error && (
        <>
      {/* Filter & Search Bar */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4 bg-slate-900/50 p-4 rounded-2xl border border-white/10">
        <div className="search-bar md:col-span-2">
          <Search className="search-icon" />
          <input
            type="text"
            placeholder="Search team member by name or designation..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />
        </div>
        <div>
          <select className="form-control" value={deptFilter} onChange={(e) => setDeptFilter(e.target.value)}>
            <option value="all" className="bg-slate-900">All Departments</option>
            <option value="engineering" className="bg-slate-900">Engineering</option>
            <option value="product" className="bg-slate-900">Product</option>
            <option value="design" className="bg-slate-900">Design</option>
          </select>
        </div>
      </div>

      {/* Team Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {filteredTeam.map((emp) => (
          <div key={emp.id} className="glass-card flex flex-col justify-between space-y-4 hover:border-teal-500/40 transition-all">
            <div>
              <div className="flex items-center justify-between mb-4 border-b border-white/10 pb-4">
                <div className="flex items-center gap-3">
                  <div className={`avatar avatar-lg ${emp.color} text-white font-bold`}>{emp.avatar}</div>
                  <div>
                    <h3 className="font-bold text-white text-base">{emp.name}</h3>
                    <p className="text-xs text-teal-400 font-medium">{emp.designation}</p>
                  </div>
                </div>
                <span className={`badge ${emp.status === 'active' ? 'badge-success' : 'badge-warning'}`}>{emp.status.toUpperCase()}</span>
              </div>
              <div className="space-y-2 text-xs text-slate-400">
                <div className="flex items-center gap-2"><Mail className="w-3.5 h-3.5 text-slate-500" /> <span className="text-slate-200">{emp.email}</span></div>
                <div className="flex items-center gap-2"><MapPin className="w-3.5 h-3.5 text-slate-500" /> <span className="text-slate-200">{emp.location}</span></div>
                <div className="flex items-center gap-2"><Calendar className="w-3.5 h-3.5 text-slate-500" /> <span>Joined {emp.joinDate}</span></div>
              </div>
            </div>
            <div className="border-t border-white/10 pt-3 flex items-center justify-between text-xs">
              <div><span className="text-slate-500 block">Reporting Manager</span><strong className="text-slate-300">{emp.reportingManager}</strong></div>
              <button className="btn-teal py-1.5 px-3 text-xs" onClick={() => setSelectedEmp(emp)}>View Profile</button>
            </div>
          </div>
        ))}
      </div>

      {/* Employee Modal */}
      {selectedEmp && (
        <div className="modal-backdrop">
          <div className="modal-panel max-w-lg">
            <div className="modal-header">
              <div className="flex items-center gap-3">
                <div className={`avatar avatar-lg ${selectedEmp.color} text-white font-bold`}>{selectedEmp.avatar}</div>
                <div>
                  <h3 className="text-xl font-bold text-white">{selectedEmp.name}</h3>
                  <p className="text-xs text-teal-400 font-medium">{selectedEmp.designation} · {selectedEmp.department}</p>
                </div>
              </div>
              <button onClick={() => setSelectedEmp(null)} className="text-slate-400 hover:text-white">&times;</button>
            </div>
            <div className="modal-body space-y-4 text-sm">
              <div className="bg-slate-900 p-4 rounded-xl border border-white/10 space-y-2 text-xs">
                <div><span className="text-slate-400 block mb-0.5">Corporate Email</span><strong className="text-white text-sm">{selectedEmp.email}</strong></div>
                <div><span className="text-slate-400 block mb-0.5">Work Location</span><strong className="text-white text-sm">{selectedEmp.location}</strong></div>
                <div><span className="text-slate-400 block mb-0.5">Join Date</span><strong className="text-white text-sm">{selectedEmp.joinDate}</strong></div>
              </div>
              <div className="p-4 rounded-xl bg-teal-500/10 border border-teal-500/20 space-y-1 text-xs">
                <div className="text-teal-400 font-bold uppercase tracking-wider mb-2">Hierarchy & Reporting Chain</div>
                <div>Direct Manager: <strong>{selectedEmp.manager}</strong></div>
                <div>Reporting Manager / Approver: <strong>{selectedEmp.reportingManager}</strong></div>
                <div>Global Tier: <strong>Department Level Core Roster</strong></div>
              </div>
            </div>
            <div className="modal-footer">
              <button className="btn-ghost" onClick={() => setSelectedEmp(null)}>Close Profile</button>
            </div>
          </div>
        </div>
      )}
      </>
      )}
    </div>
  );
}
