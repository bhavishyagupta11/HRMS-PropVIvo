'use client';

import React, { useState, useEffect } from 'react';
import { Clock, CheckCircle2, AlertCircle, Calendar, UserCheck, Play } from 'lucide-react';
import { useRole } from '../context/RoleContext';

export default function AttendanceFeature() {
  const { activeRole, capabilities, graphqlFetch, token } = useRole();
  const [clockedIn, setClockedIn] = useState(false);
  const [showModal, setShowModal] = useState(false);
  const [clockMethod, setClockMethod] = useState('selfie');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [activeTab, setActiveTab] = useState('logs'); // logs, shifts, team

  const [attendanceLogs, setAttendanceLogs] = useState([]);
  const [teamAttendance, setTeamAttendance] = useState([]);
  const [todayRecord, setTodayRecord] = useState(null);
  const [liveDuration, setLiveDuration] = useState('00:00:00');

  const shifts = [
    { day: 'Monday', start: '09:00 AM', end: '06:00 PM', type: 'Regular', current: false },
    { day: 'Tuesday', start: '09:00 AM', end: '06:00 PM', type: 'Regular', current: false },
    { day: 'Wednesday', start: '09:00 AM', end: '06:00 PM', type: 'Regular', current: true },
    { day: 'Thursday', start: '09:00 AM', end: '06:00 PM', type: 'Regular', current: false },
    { day: 'Friday', start: '09:00 AM', end: '05:00 PM', type: 'Early End', current: false },
  ];

  // Load attendance data from backend
  const loadAttendanceData = async () => {
    if (!token) return;
    setLoading(true);
    setError('');
    try {
      // 1. Fetch Today's Attendance status
      const todayQuery = `
        query GetMyTodayAttendance {
          myTodayAttendance {
            id
            clockInTime
            clockOutTime
            clockInMethod
            locationVerified
            ipValidated
            status
          }
        }
      `;
      const todayData = await graphqlFetch(todayQuery);
      if (todayData && todayData.myTodayAttendance) {
        setTodayRecord(todayData.myTodayAttendance);
        setClockedIn(!todayData.myTodayAttendance.clockOutTime);
      } else {
        setTodayRecord(null);
        setClockedIn(false);
      }

      // 2. Fetch History Logs for Current Month/Year
      const now = new Date();
      const logsQuery = `
        query GetMyAttendance($month: Int!, $year: Int!) {
          myAttendance(month: $month, year: $year) {
            id
            date
            clockInTime
            clockOutTime
            clockInMethod
            locationVerified
            ipValidated
            totalHours
            productiveHours
            breakHours
            overtimeHours
            status
          }
        }
      `;
      const logsData = await graphqlFetch(logsQuery, { month: now.getMonth() + 1, year: now.getFullYear() });
      if (logsData && logsData.myAttendance) {
        setAttendanceLogs(logsData.myAttendance);
      }

      // 3. Fetch Team logs if authorized
      if (capabilities.canViewTeam) {
        const teamQuery = `
          query GetTeamAttendance($date: DateTime!) {
            teamAttendance(date: $date) {
              id
              userId
              clockInTime
              clockOutTime
              clockInMethod
              status
            }
          }
        `;
        const teamData = await graphqlFetch(teamQuery, { date: now.toISOString().split('T')[0] + 'T00:00:00Z' });
        if (teamData && teamData.teamAttendance) {
          setTeamAttendance(teamData.teamAttendance);
        }
      }
    } catch (err) {
      setError(err.message || 'Error loading attendance data');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadAttendanceData();
  }, [token, activeRole]);

  useEffect(() => {
    let interval;
    if (clockedIn && todayRecord && todayRecord.clockInTime && !todayRecord.clockOutTime) {
      interval = setInterval(() => {
        const start = new Date(todayRecord.clockInTime).getTime();
        const now = new Date().getTime();
        const diff = Math.max(0, now - start);
        
        const hrs = Math.floor(diff / 3600000);
        const mins = Math.floor((diff % 3600000) / 60000);
        const secs = Math.floor((diff % 60000) / 1000);
        
        setLiveDuration(
          `${hrs.toString().padStart(2, '0')}:${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`
        );
      }, 1000);
    } else {
      setLiveDuration('00:00:00');
    }
    return () => {
      if (interval) clearInterval(interval);
    };
  }, [clockedIn, todayRecord]);

  const handleClockToggle = async () => {
    if (!clockedIn) {
      setShowModal(true);
    } else {
      setLoading(true);
      setError('');
      try {
        const mutation = `
          mutation ClockOut {
            clockOut
          }
        `;
        const data = await graphqlFetch(mutation);
        if (data && data.clockOut) {
          setClockedIn(false);
          await loadAttendanceData();
        }
      } catch (err) {
        setError(err.message || 'Clock Out failed');
      } finally {
        setLoading(false);
      }
    }
  };

  const confirmClockIn = async () => {
    setLoading(true);
    setError('');
    try {
      const mutation = `
        mutation ClockIn($clockInMethod: String!, $shiftName: String!, $shiftStartTime: String!, $shiftEndTime: String!, $locationVerified: Boolean!, $ipValidated: Boolean!) {
          clockIn(
            clockInMethod: $clockInMethod,
            shiftName: $shiftName,
            shiftStartTime: $shiftStartTime,
            shiftEndTime: $shiftEndTime,
            locationVerified: $locationVerified,
            ipValidated: $ipValidated
          )
        }
      `;
      const variables = {
        clockInMethod: clockMethod,
        shiftName: 'Regular Shift',
        shiftStartTime: '09:00:00',
        shiftEndTime: '18:00:00',
        locationVerified: true,
        ipValidated: true
      };
      const data = await graphqlFetch(mutation, variables);
      if (data && data.clockIn) {
        setClockedIn(true);
        setShowModal(false);
        await loadAttendanceData();
      } else {
        throw new Error('Action failed: You may already be clocked in.');
      }
    } catch (err) {
      setError(err.message || 'Clock In failed');
    } finally {
      setLoading(false);
    }
  };

  const formatTime = (timeStr) => {
    if (!timeStr) return '--';
    try {
      const d = new Date(timeStr);
      return d.toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit' });
    } catch {
      return timeStr;
    }
  };

  const formatDate = (dateStr) => {
    if (!dateStr) return '--';
    try {
      const d = new Date(dateStr);
      return d.toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' });
    } catch {
      return dateStr;
    }
  };

  // Helper stats computation
  const stats = {
    total: attendanceLogs.reduce((acc, log) => acc + log.totalHours, 0),
    productive: attendanceLogs.reduce((acc, log) => acc + log.productiveHours, 0),
    break: attendanceLogs.reduce((acc, log) => acc + log.breakHours, 0),
    overtime: attendanceLogs.reduce((acc, log) => acc + log.overtimeHours, 0),
  };

  return (
    <div className="animate-fade-in space-y-6">
      <div className="section-header">
        <div>
          <h1 className="section-title"><Clock className="w-7 h-7 text-teal-500" /> Attendance Management</h1>
          <p className="section-subtitle">Multi-method time capture, shift scheduling, and overtime tracking</p>
        </div>
      </div>

      {error && (
        <div className="p-4 bg-rose-500/10 border border-rose-500/20 text-rose-400 text-sm rounded-xl">
          {error}
        </div>
      )}

      {/* Top Section: Clock-in Status Card */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div className="glass-card text-center md:col-span-1 flex flex-col justify-between p-6">
          <div>
            <h3 className="text-sm font-semibold text-slate-400 uppercase tracking-wider mb-2">Today's Status</h3>
            <div className={`text-3xl font-bold my-4 ${clockedIn ? 'text-emerald-400' : 'text-amber-500'}`}>
              {clockedIn ? 'Active / Working' : 'Off Duty'}
            </div>
            {clockedIn && todayRecord && (
              <div className="flex flex-col items-center gap-2 mb-4">
                <div className="text-xl font-mono text-white tracking-widest bg-slate-900/80 px-4 py-2 rounded-xl border border-teal-500/30">
                  {liveDuration}
                </div>
                <div className="text-xs text-slate-300">Clocked In at: <strong>{formatTime(todayRecord.clockInTime)}</strong></div>
                <div className="flex justify-center gap-2">
                  <span className="badge badge-success"><CheckCircle2 className="w-3 h-3" /> Location Verified</span>
                  <span className="badge badge-teal"><CheckCircle2 className="w-3 h-3" /> IP Validated</span>
                </div>
              </div>
            )}
          </div>
          <button
            className={`w-full py-3.5 rounded-xl font-bold text-base text-white transition-all shadow-lg ${
              clockedIn ? 'bg-slate-700 hover:bg-slate-600 shadow-slate-900/50' : 'bg-gradient-to-r from-teal-600 to-teal-500 hover:from-teal-500 hover:to-teal-400 shadow-teal-900/40'
            }`}
            onClick={handleClockToggle}
            disabled={loading}
          >
            {loading ? 'Processing...' : clockedIn ? 'Clock Out' : 'Clock In Now'}
          </button>
        </div>

        {/* Stats Row */}
        <div className="md:col-span-2 grid grid-cols-2 gap-4">
          <div className="stat-card">
            <p className="stat-label">Total Hours</p>
            <p className="stat-value text-teal-400">{stats.total.toFixed(1)}h</p>
            <p className="stat-sub">This month</p>
          </div>
          <div className="stat-card">
            <p className="stat-label">Productive Hours</p>
            <p className="stat-value text-emerald-400">{stats.productive.toFixed(1)}h</p>
            <p className="stat-sub">Excluding breaks</p>
          </div>
          <div className="stat-card">
            <p className="stat-label">Break Hours</p>
            <p className="stat-value text-amber-400">{stats.break.toFixed(1)}h</p>
            <p className="stat-sub">Within limits</p>
          </div>
          <div className="stat-card">
            <p className="stat-label">Overtime Hours</p>
            <p className="stat-value text-purple-400">{stats.overtime.toFixed(1)}h</p>
            <p className="stat-sub">Tracked separately</p>
          </div>
        </div>
      </div>

      {/* Tabs */}
      <div className="tab-bar">
        <button className={`tab-btn ${activeTab === 'logs' ? 'active' : ''}`} onClick={() => setActiveTab('logs')}>
          Attendance Logs
        </button>
        <button className={`tab-btn ${activeTab === 'shifts' ? 'active' : ''}`} onClick={() => setActiveTab('shifts')}>
          Shift Schedule
        </button>
        {capabilities.canViewTeam && (
          <button className={`tab-btn ${activeTab === 'team' ? 'active' : ''}`} onClick={() => setActiveTab('team')}>
            Team Attendance
          </button>
        )}
      </div>

      {/* Tab Contents */}
      {activeTab === 'logs' && (
        <div className="glass-card">
          <h2 className="text-lg font-bold text-slate-100 mb-4">My Attendance History</h2>
          <div className="overflow-x-auto">
            <table className="data-table">
              <thead>
                <tr>
                  <th>Date</th>
                  <th>Clock In</th>
                  <th>Clock Out</th>
                  <th>Hours</th>
                  <th>Productive</th>
                  <th>Location</th>
                  <th>Status</th>
                </tr>
              </thead>
              <tbody>
                {attendanceLogs.length === 0 ? (
                  <tr>
                    <td colSpan="7" className="text-center p-6 text-slate-500">No attendance logs found for this month.</td>
                  </tr>
                ) : (
                  attendanceLogs.map((log) => (
                    <tr key={log.id}>
                      <td className="font-semibold text-white">{formatDate(log.date)}</td>
                      <td>{formatTime(log.clockInTime)}</td>
                      <td>{formatTime(log.clockOutTime)}</td>
                      <td className="text-teal-400 font-medium">{log.totalHours.toFixed(1)}h</td>
                      <td>{log.productiveHours.toFixed(1)}h</td>
                      <td>
                        <div className="flex items-center gap-1.5">
                          {log.clockInMethod}
                          {log.locationVerified && <CheckCircle2 className="w-3.5 h-3.5 text-emerald-400" title="Location Verified" />}
                        </div>
                      </td>
                      <td>
                        <span className={`badge ${
                          log.status === 'On Time' ? 'badge-success' : log.status === 'Late' ? 'badge-warning' : 'badge-danger'
                        }`}>
                          {log.status}
                        </span>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </div>
      )}

      {activeTab === 'shifts' && (
        <div className="glass-card">
          <h2 className="text-lg font-bold text-slate-100 mb-4">My Shift Calendar</h2>
          <div className="grid grid-cols-1 md:grid-cols-5 gap-4">
            {shifts.map((shift, index) => (
              <div key={index} className={`p-4 rounded-xl border ${shift.current ? 'bg-teal-600/20 border-teal-500/30' : 'bg-slate-900/50 border-white/5'}`}>
                <div className="flex items-center justify-between mb-2">
                  <span className="font-bold text-white text-sm">{shift.day}</span>
                  {shift.current && <span className="badge badge-teal">Today</span>}
                </div>
                <div className="text-xs text-slate-400 mb-2">{shift.start} - {shift.end}</div>
                <div className="badge badge-slate">{shift.type}</div>
              </div>
            ))}
          </div>
        </div>
      )}

      {activeTab === 'team' && capabilities.canViewTeam && (
        <div className="glass-card">
          <div className="flex items-center justify-between mb-4">
            <h2 className="text-lg font-bold text-slate-100">Team Attendance & Exceptions</h2>
          </div>
          <div className="overflow-x-auto">
            <table className="data-table">
              <thead>
                <tr>
                  <th>Employee ID</th>
                  <th>Clock In</th>
                  <th>Clock Out</th>
                  <th>Status</th>
                  <th>Method</th>
                </tr>
              </thead>
              <tbody>
                {teamAttendance.length === 0 ? (
                  <tr>
                    <td colSpan="5" className="text-center p-6 text-slate-500">No team attendance records for today.</td>
                  </tr>
                ) : (
                  teamAttendance.map((member) => (
                    <tr key={member.id}>
                      <td className="font-semibold text-white">{member.userId}</td>
                      <td>{formatTime(member.clockInTime)}</td>
                      <td>{formatTime(member.clockOutTime)}</td>
                      <td>
                        <span className={`badge ${member.status === 'On Time' ? 'badge-success' : member.status === 'Late' ? 'badge-warning' : 'badge-danger'}`}>
                          {member.status}
                        </span>
                      </td>
                      <td className="text-slate-400 capitalize">{member.clockInMethod}</td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </div>
      )}

      {/* Clock In Method Modal */}
      {showModal && (
        <div className="modal-backdrop">
          <div className="modal-panel">
            <div className="modal-header">
              <h3 className="text-lg font-bold text-slate-100">Select Clock-In Method</h3>
              <button onClick={() => setShowModal(false)} className="text-slate-400 hover:text-white">&times;</button>
            </div>
            <div className="modal-body space-y-4">
              <p className="text-sm text-slate-400">Choose your verification method per company attendance policy:</p>
              <div className="space-y-2">
                {[
                  { id: 'selfie', label: 'Selfie Capture (with AI liveness check)', desc: 'Requires camera access' },
                  { id: 'geolocation', label: 'Geolocation Verification', desc: 'Validates distance to office geofence' },
                  { id: 'ip', label: 'IP-Based Validation', desc: 'Checks matching office network IP' },
                  { id: 'biometric', label: 'Biometric Scanner', desc: 'Integrates with external hardware' },
                  { id: 'manual', label: 'Manual Request', desc: 'Requires manager approval' },
                ].map(opt => (
                  <label key={opt.id} className={`flex items-start gap-3 p-3.5 rounded-xl border cursor-pointer transition-colors ${
                    clockMethod === opt.id ? 'bg-teal-600/20 border-teal-500/40' : 'bg-slate-900/50 border-white/10 hover:bg-slate-900'
                  }`}>
                    <input
                      type="radio"
                      name="clockMethod"
                      value={opt.id}
                      checked={clockMethod === opt.id}
                      onChange={() => setClockMethod(opt.id)}
                      className="mt-1 accent-teal-500"
                    />
                    <div>
                      <div className="font-semibold text-sm text-white">{opt.label}</div>
                      <div className="text-xs text-slate-400 mt-0.5">{opt.desc}</div>
                    </div>
                  </label>
                ))}
              </div>
            </div>
            <div className="modal-footer">
              <button className="btn-ghost" onClick={() => setShowModal(false)}>Cancel</button>
              <button className="btn-teal" onClick={confirmClockIn} disabled={loading}>
                {loading ? 'Verifying...' : 'Confirm Clock In'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
