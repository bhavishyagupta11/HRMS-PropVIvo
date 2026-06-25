'use client';

import React, { useState, useEffect } from 'react';
import { DollarSign, Download, ShieldCheck, FileText, CheckCircle2 } from 'lucide-react';
import { useRole } from '../context/RoleContext';

export default function PayrollFeature() {
  const { graphqlFetch, token } = useRole();
  const [activeTab, setActiveTab] = useState('current'); // current, history, taxes, compliance
  const [country, setCountry] = useState('IN'); // IN, US

  const [payslips, setPayslips] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const loadPayrollData = async () => {
    if (!token) return;
    setLoading(true);
    setError('');
    try {
      const query = `
        query GetMyPayslips {
          myPayslips {
            id
            payPeriod
            payDate
            countryCode
            status
            grossPay
            totalDeductions
            netPay
            earningsJson
            deductionsJson
            employerContributionsJson
          }
        }
      `;
      const data = await graphqlFetch(query);
      if (data && data.myPayslips) {
        setPayslips(data.myPayslips);
      }
    } catch (err) {
      setError(err.message || 'Error loading payslips');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadPayrollData();
  }, [token]);

  // Parse custom JSON helpers
  const parseJson = (str, def = []) => {
    if (!str) return def;
    try {
      return JSON.parse(str) || def;
    } catch {
      return def;
    }
  };

  const handleDownloadFile = (fileName, content) => {
    const blob = new Blob([content], { type: 'text/plain' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `${fileName}.txt`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(url);
  };

  // Filter payslips for the selected country compliance
  const filteredSlips = payslips
    .filter(p => p.countryCode.toUpperCase() === country.toUpperCase())
    .sort((a, b) => new Date(b.payDate) - new Date(a.payDate));

  const latestPayslip = filteredSlips[0] || null;

  const payslipHistory = filteredSlips.map(p => ({
    period: p.payPeriod,
    grossPay: p.grossPay,
    netPay: p.netPay,
    status: p.status.toLowerCase(),
    payDate: p.payDate.split('T')[0]
  }));

  const taxDocuments = country === 'IN' ? [
    { id: 'TAX-1', name: 'Form 16 FY 2025-26', type: 'Tax', status: 'Available', date: '2026-06-10' },
    { id: 'TAX-2', name: 'Form 12BA FY 2025-26', type: 'Perquisites', status: 'Available', date: '2026-06-10' },
  ] : [
    { id: 'TAX-1', name: 'Form W-2 (Wage & Tax Statement)', type: 'Tax', status: 'Available', date: '2026-06-10' },
    { id: 'TAX-2', name: 'Form 1095-C (Health Coverage)', type: 'Insurance', status: 'Available', date: '2026-06-10' },
  ];

  const complianceItems = country === 'IN' ? [
    { id: 'COMP-1', statutoryTitle: 'Provident Fund (PF) Remittance', period: 'May 2026', filingDate: '2026-06-12', status: 'Filed Successfully', refNo: 'PF-TRN-99882341' },
    { id: 'COMP-2', statutoryTitle: 'Employee State Insurance (ESI) Filing', period: 'May 2026', filingDate: '2026-06-14', status: 'Filed Successfully', refNo: 'ESI-TRN-77665544' },
    { id: 'COMP-3', statutoryTitle: 'Income Tax (TDS) Monthly Challan', period: 'May 2026', filingDate: '2026-06-07', status: 'Filed Successfully', refNo: 'TDS-CHL-11223344' },
    { id: 'COMP-4', statutoryTitle: 'Professional Tax Return', period: 'Q1 2026', filingDate: '2026-06-25', status: 'Pending Filing', refNo: '--' },
  ] : [
    { id: 'COMP-1', statutoryTitle: 'Form 941 Quarterly Federal Return', period: 'Q1 2026', filingDate: '2026-06-20', status: 'Filed Successfully', refNo: 'US-941-88775432' },
    { id: 'COMP-2', statutoryTitle: 'FUTA Tax Deposit (Form 940)', period: 'May 2026', filingDate: '2026-06-15', status: 'Filed Successfully', refNo: 'US-940-11229988' },
    { id: 'COMP-3', statutoryTitle: 'State Unemployment (SUTA) Remittance', period: 'May 2026', filingDate: '2026-06-18', status: 'Filed Successfully', refNo: 'US-SUTA-33442211' },
    { id: 'COMP-4', statutoryTitle: 'Form 1094-C Transmittal Filing', period: 'Q1 2026', filingDate: '2026-06-25', status: 'Pending Filing', refNo: '--' },
  ];

  const formatCurr = (amt) => country === 'IN' ? `₹${parseFloat(amt).toLocaleString('en-IN')}` : `$${parseFloat(amt).toLocaleString('en-US')}`;

  return (
    <div className="animate-fade-in space-y-6">
      <div className="section-header flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="section-title"><DollarSign className="w-7 h-7 text-teal-500" /> Payroll & Compliance</h1>
          <p className="section-subtitle">Localized payroll with detailed payslips, statutory deductions, and tax compliance</p>
        </div>
        <div className="flex items-center gap-2 bg-slate-900/60 p-1.5 rounded-2xl border border-white/10 self-start sm:self-center">
          <button
            onClick={() => setCountry('IN')}
            className={`px-3.5 py-2 rounded-xl text-xs font-bold transition-all flex items-center gap-1.5 ${country === 'IN' ? 'bg-teal-500 text-white' : 'text-slate-400 hover:text-slate-200'}`}
          >
            <span>🇮🇳</span> India Compliance
          </button>
          <button
            onClick={() => setCountry('US')}
            className={`px-3.5 py-2 rounded-xl text-xs font-bold transition-all flex items-center gap-1.5 ${country === 'US' ? 'bg-teal-500 text-white' : 'text-slate-400 hover:text-slate-200'}`}
          >
            <span>🇺🇸</span> US Compliance
          </button>
        </div>
      </div>

      {error && (
        <div className="p-4 bg-rose-500/10 border border-rose-500/20 text-rose-400 text-sm rounded-xl">
          {error}
        </div>
      )}

      {/* Tabs */}
      <div className="tab-bar">
        <button className={`tab-btn ${activeTab === 'current' ? 'active' : ''}`} onClick={() => setActiveTab('current')}>
          Current Payslip
        </button>
        <button className={`tab-btn ${activeTab === 'history' ? 'active' : ''}`} onClick={() => setActiveTab('history')}>
          Pay History
        </button>
        <button className={`tab-btn ${activeTab === 'taxes' ? 'active' : ''}`} onClick={() => setActiveTab('taxes')}>
          Tax Documents
        </button>
        <button className={`tab-btn ${activeTab === 'compliance' ? 'active' : ''}`} onClick={() => setActiveTab('compliance')}>
          Statutory Compliance
        </button>
      </div>

      {loading ? (
        <div className="p-12 text-center text-slate-400">Loading payroll details...</div>
      ) : (
        <>
          {activeTab === 'current' && (
            <div className="space-y-6">
              {latestPayslip ? (
                <div className="glass-card max-w-3xl mx-auto border border-white/10 shadow-2xl rounded-3xl overflow-hidden p-6 md:p-8 space-y-6">
                  {/* Payslip Header */}
                  <div className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4 border-b border-white/10 pb-6">
                    <div>
                      <h2 className="text-xl font-bold text-white">Salary Statement</h2>
                      <p className="text-xs text-slate-400 mt-1">Pay Period: <strong>{latestPayslip.payPeriod}</strong></p>
                    </div>
                    <div className="text-right">
                      <div className="text-xs text-slate-400">Payment Date: {latestPayslip.payDate.split('T')[0]}</div>
                      <span className="badge badge-success mt-1.5 uppercase font-bold text-[10px] tracking-wider">{latestPayslip.status}</span>
                    </div>
                  </div>

                  {/* Summary row */}
                  <div className="grid grid-cols-3 gap-4 bg-slate-950/40 p-4 rounded-2xl border border-white/5">
                    <div>
                      <div className="text-slate-400 text-[10px] uppercase font-bold tracking-wider">Gross Pay</div>
                      <div className="text-lg font-bold text-white mt-1">{formatCurr(latestPayslip.grossPay)}</div>
                    </div>
                    <div>
                      <div className="text-slate-400 text-[10px] uppercase font-bold tracking-wider">Deductions</div>
                      <div className="text-lg font-bold text-rose-400 mt-1">{formatCurr(latestPayslip.totalDeductions)}</div>
                    </div>
                    <div>
                      <div className="text-slate-400 text-[10px] uppercase font-bold tracking-wider">Net Take-Home</div>
                      <div className="text-lg font-bold text-emerald-400 mt-1">{formatCurr(latestPayslip.netPay)}</div>
                    </div>
                  </div>

                  {/* Breakdown columns */}
                  <div className="grid grid-cols-1 md:grid-cols-2 gap-6 pt-4">
                    {/* Earnings */}
                    <div className="space-y-4">
                      <h3 className="font-bold text-white text-sm border-b border-white/5 pb-2">Earnings & Allowances</h3>
                      <div className="space-y-2">
                        {parseJson(latestPayslip.earningsJson).map((item, idx) => (
                          <div key={idx} className="flex justify-between text-xs">
                            <span className="text-slate-400">{item.label}</span>
                            <span className="font-semibold text-white">{formatCurr(item.amount)}</span>
                          </div>
                        ))}
                      </div>
                    </div>

                    {/* Deductions */}
                    <div className="space-y-4">
                      <h3 className="font-bold text-white text-sm border-b border-white/5 pb-2">Statutory Deductions</h3>
                      <div className="space-y-2">
                        {parseJson(latestPayslip.deductionsJson).map((item, idx) => (
                          <div key={idx} className="flex justify-between text-xs">
                            <span className="text-slate-400">{item.label}</span>
                            <span className="font-semibold text-rose-400">{formatCurr(item.amount)}</span>
                          </div>
                        ))}
                      </div>
                    </div>
                  </div>

                  {/* Employer contributions */}
                  <div className="border-t border-white/10 pt-6 space-y-4">
                    <h3 className="font-bold text-white text-sm">Employer Statutory Contributions</h3>
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                      {parseJson(latestPayslip.employerContributionsJson).map((item, idx) => (
                        <div key={idx} className="flex justify-between text-xs bg-slate-900/40 p-3 rounded-xl border border-white/5">
                          <span className="text-slate-400">{item.label}</span>
                          <span className="font-semibold text-teal-400">{formatCurr(item.amount)}</span>
                        </div>
                      ))}
                    </div>
                  </div>
                </div>
              ) : (
                <div className="p-8 text-center text-slate-500 italic space-y-2">
                  <p>No payslip records available for the selected country context ({country === 'IN' ? 'India' : 'US'}).</p>
                  <p className="text-xs">This may be because you have not been processed in this region's payroll cycle yet.</p>
                </div>
              )}
            </div>
          )}

          {activeTab === 'history' && (
            <div className="glass-card">
              <h2 className="text-lg font-bold text-slate-100 mb-4">Pay Statement History</h2>
              <div className="overflow-x-auto">
                <table className="data-table">
                  <thead>
                    <tr>
                      <th>Pay Period</th>
                      <th>Payment Date</th>
                      <th>Gross Pay</th>
                      <th>Net Take-Home</th>
                      <th>Status</th>
                      <th className="text-right">Action</th>
                    </tr>
                  </thead>
                  <tbody>
                    {payslipHistory.length === 0 ? (
                      <tr>
                        <td colSpan="6" className="text-center p-6 text-slate-500">No statement history found.</td>
                      </tr>
                    ) : (
                      payslipHistory.map((item, idx) => (
                        <tr key={idx}>
                          <td className="font-bold text-white">{item.period}</td>
                          <td>{item.payDate}</td>
                          <td>{formatCurr(item.grossPay)}</td>
                          <td className="text-emerald-400 font-semibold">{formatCurr(item.netPay)}</td>
                          <td>
                            <span className="badge badge-success capitalize">{item.status}</span>
                          </td>
                          <td className="text-right">
                            <button 
                              className="p-1.5 rounded-lg bg-teal-500/10 hover:bg-teal-500/20 text-teal-400 border border-teal-500/20 transition-all"
                              onClick={() => handleDownloadFile(`Payslip_${item.period}`, `Payslip for ${item.period}\nGross: ${item.grossPay}\nNet: ${item.netPay}\nStatus: ${item.status}`)}
                            >
                              <Download className="w-4 h-4" />
                            </button>
                          </td>
                        </tr>
                      ))
                    )}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {activeTab === 'taxes' && (
            <div className="glass-card">
              <h2 className="text-lg font-bold text-slate-100 mb-4">Annual Tax Forms & Statements</h2>
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                {taxDocuments.map((doc) => (
                  <div key={doc.id} className="p-5 bg-slate-900/60 border border-white/10 rounded-2xl flex items-center justify-between">
                    <div className="flex items-center gap-3">
                      <div className="w-10 h-10 rounded-xl bg-teal-500/10 flex items-center justify-center text-teal-400">
                        <FileText className="w-5 h-5" />
                      </div>
                      <div>
                        <h4 className="font-bold text-white text-sm">{doc.name}</h4>
                        <p className="text-[10px] text-slate-400 mt-0.5">Available since: {doc.date}</p>
                      </div>
                    </div>
                    <button 
                      className="btn-teal py-2 text-xs flex items-center gap-1.5"
                      onClick={() => handleDownloadFile(doc.name.replace(/ /g, '_'), `Tax Document: ${doc.name}\nType: ${doc.type}\nDate: ${doc.date}\nStatus: ${doc.status}\n\nThis is a securely generated statutory document.`)}
                    >
                      <Download className="w-3.5 h-3.5" /> Download
                    </button>
                  </div>
                ))}
              </div>
            </div>
          )}

          {activeTab === 'compliance' && (
            <div className="glass-card">
              <h2 className="text-lg font-bold text-slate-100 mb-4">Statutory Filings Compliance Board</h2>
              <div className="space-y-4">
                {complianceItems.map((item) => (
                  <div key={item.id} className="p-4 rounded-xl border border-white/5 bg-slate-900/30 flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3">
                    <div>
                      <h4 className="font-bold text-white text-sm">{item.statutoryTitle}</h4>
                      <div className="text-xs text-slate-400 mt-1">Period: <strong>{item.period}</strong> • Filed: {item.filingDate}</div>
                    </div>
                    <div className="flex items-center gap-4">
                      <div className="text-right">
                        <span className={`badge ${item.refNo !== '--' ? 'badge-success' : 'badge-warning'}`}>{item.status}</span>
                        {item.refNo !== '--' && <div className="text-[10px] text-slate-500 mt-1">Ref: {item.refNo}</div>}
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          )}
        </>
      )}
    </div>
  );
}
