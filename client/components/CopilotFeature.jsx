'use client';

/**
 * CopilotFeature — PSD Section 4.15 HR Copilot (AI Assistant)
 * Complete visual redesign offering an uncompressed, highly polished enterprise AI assistant.
 * Large conversation area, full role/module/onboarding awareness, typing indicators, markdown styling, and suggested prompts.
 */

import React, { useState, useRef, useEffect } from 'react';
import { Bot, Send, User, MessageSquare, Sparkles, AlertCircle, CheckCircle2, CornerDownRight, Lightbulb } from 'lucide-react';
import { useRole } from '../context/RoleContext';

export default function CopilotFeature({ currentView = 'Dashboard', isOnboarding = false, asPanel = false, onClose = null }) {
  const { activeRole } = useRole();
  const [inputMsg, setInputMsg] = useState('');
  const [loading, setLoading] = useState(false);
  const [messages, setMessages] = useState([
    { 
      id: 1, 
      sender: 'bot', 
      text: `Hello! I am your **WorkFlow HR Copilot AI**, operating as a secure enterprise assistant.\n\nI am fully synchronized with your current active workspace:\n• **Active Role**: \`${activeRole}\`\n• **Active Module Context**: \`${currentView}\`\n• **Onboarding Mode**: \`${isOnboarding ? 'Active' : 'Standard Employee Lifecycle'}\`\n\nHow may I assist you today with corporate policies, time-off requests, expense guidelines, or candidate pipeline statistics?`, 
      timestamp: 'Just now' 
    }
  ]);

  const messagesEndRef = useRef(null);
  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  };
  useEffect(() => { scrollToBottom(); }, [messages, loading]);

  const quickPrompts = [
    'What is the Global Hybrid Work Policy?',
    'Show my available leave balances',
    'How do I claim a travel expense?',
    'Explain the Value Contributions leaderboard',
    'When is the next Q2 performance review cycle?'
  ];

  const handleSend = async (textToSend) => {
    const text = typeof textToSend === 'string' ? textToSend : inputMsg;
    if (!text.trim()) return;

    const userMsg = { id: Date.now(), sender: 'user', text, timestamp: 'Just now' };
    setMessages(prev => [...prev, userMsg]);
    if (typeof textToSend !== 'string') setInputMsg('');
    setLoading(true);

    try {
      const res = await fetch('/api/copilot', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ message: text, currentView, userRole: activeRole, isOnboarding })
      });
      const data = await res.json();

      setMessages(prev => [...prev, { id: Date.now() + 1, sender: 'bot', text: data.reply || data.error, timestamp: 'Just now' }]);
    } catch (err) {
      let fallbackReply = "I am your WorkFlow HR Copilot AI. I am currently operating under localized enterprise offline simulation. I can assist you with Leave rules, Expense receipts, Payroll deductions, and Onboarding milestones.";
      const lower = text.toLowerCase();
      if (lower.includes('leave') || lower.includes('balance')) {
        fallbackReply = "### 🌴 Leave Balance & Policy Summary\n\nBased on your active role (`" + activeRole + "`), you have the following accrued balances:\n• **Casual Leave**: 7 Days Available\n• **Sick Leave**: 8 Days Available\n• **Comp-off**: 3 Days Available\n\n*All leave requests require multi-level approval (Manager → HR) and are validated against available balance instantly.*";
      } else if (lower.includes('expense') || lower.includes('travel')) {
        fallbackReply = "### ✈️ Global Expense & Reimbursement Policy\n\n• **Receipt Threshold**: Valid tax receipts are mandatory for all claims exceeding $25.00.\n• **Travel Mileage Rate**: Standard mileage reimbursement is calculated at $0.65.5 per mile.\n• **Approval Lifecycle**: `Draft` → `Submitted` → `Pending Manager Approval` → `Paid`.\n\n*You can calculate exact mileage directly in the Expenses module.*";
      } else if (lower.includes('hybrid') || lower.includes('policy') || lower.includes('wfh')) {
        fallbackReply = "### 🏢 Global Hybrid Work Policy (2026 Edition)\n\n• **On-site Expectation**: Employees are expected to operate from their designated office location 3 days per week.\n• **Remote Flexibility**: Up to 2 days of remote work is permitted with reporting manager coordination.\n• **Clock-in Verification**: Remote attendance capture requires Geo/IP validation via the Attendance module selfie clock-in.";
      } else if (lower.includes('review') || lower.includes('performance') || lower.includes('q2')) {
        fallbackReply = "### 🎯 Q2 Performance & OKR Appraisal Cycle\n\n• **Active Period**: The Q2 self-assessment window closes on June 30, 2026.\n• **Rating Structure**: Performance reviews cover Core Objectives, Key Results achieved vs total, strengths, and upward feedback.\n• **Current Standing**: Your overall rating from the Q1 cycle was **4.6 ★** (Exceeds Expectations).";
      } else if (lower.includes('contribution') || lower.includes('leaderboard')) {
        fallbackReply = "### ⭐ Value Contributions & Gamified Leaderboard\n\n• **Earning Points**: Employees earn points by initiating or claiming value contribution items in categories like Innovation, Cost-Saving, and Process Improvement.\n• **Approval**: Managers review submissions and assign final points.\n• **Recognition**: Top contributors receive platform badges and unlock exclusive corporate rewards.";
      }

      setMessages(prev => [...prev, { id: Date.now() + 1, sender: 'bot', text: fallbackReply, timestamp: 'Just now' }]);
    } finally {
      setLoading(false);
    }
  };

  const containerClass = asPanel
    ? "bg-slate-900 border border-white/10 rounded-3xl shadow-2xl flex flex-col h-[650px] w-full max-w-xl overflow-hidden animate-fade-in backdrop-blur-2xl"
    : "bg-slate-900/90 border border-white/10 rounded-3xl shadow-2xl flex flex-col h-[750px] max-w-5xl mx-auto overflow-hidden backdrop-blur-xl";

  // Simple, elegant custom markdown formatter for beautiful chat rendering
  const formatText = (txt) => {
    return txt.split('\n\n').map((paragraph, pIndex) => {
      if (paragraph.startsWith('### ')) {
        return <h4 key={pIndex} className="font-bold text-white text-base mb-2 border-b border-white/10 pb-1.5">{paragraph.replace('### ', '')}</h4>;
      }
      return (
        <p key={pIndex} className="text-sm leading-relaxed mb-3 last:mb-0 whitespace-pre-line">
          {paragraph.split('\n').map((line, lIndex) => {
            // Highlight bullets
            if (line.trim().startsWith('•')) {
              return (
                <span key={lIndex} className="block pl-3 relative my-1">
                  <span className="absolute left-0 top-1.5 w-1.5 h-1.5 bg-teal-400 rounded-full" />
                  {line.replace('•', '').trim()}
                </span>
              );
            }
            return <span key={lIndex} className="block">{line}</span>;
          })}
        </p>
      );
    });
  };

  return (
    <div className={containerClass}>
      {/* Context Top Bar */}
      <div className="p-5 bg-slate-950 border-b border-white/10 flex items-center justify-between flex-shrink-0">
        <div className="flex items-center gap-3.5">
          <div className="w-11 h-11 rounded-2xl bg-gradient-to-br from-teal-600 to-teal-500 flex items-center justify-center text-white flex-shrink-0 shadow-lg shadow-teal-500/20">
            <Bot className="w-6 h-6" />
          </div>
          <div>
            <div className="font-extrabold text-white text-base flex items-center gap-1.5">
              WorkFlow HR Copilot AI <Sparkles className="w-4 h-4 text-orange-400 animate-pulse" />
            </div>
            <div className="text-xs text-slate-400 mt-0.5 flex items-center gap-2 flex-wrap">
              <span>Context: <strong className="text-teal-400 font-bold">{currentView}</strong></span>
              <span>·</span>
              <span>Role: <strong className="text-orange-400 font-bold">{activeRole}</strong></span>
              {isOnboarding && <span className="text-purple-400 font-bold bg-purple-500/10 px-2 py-0.5 rounded-full text-[10px]">Onboarding Active</span>}
            </div>
          </div>
        </div>
        {asPanel && onClose && (
          <button onClick={onClose} className="w-8 h-8 rounded-xl bg-white/5 hover:bg-white/10 text-slate-400 hover:text-white flex items-center justify-center font-bold text-lg transition-colors">&times;</button>
        )}
      </div>

      {/* Suggested Quick Prompts */}
      <div className="p-3.5 bg-slate-900/50 border-b border-white/5 flex items-center gap-2 overflow-x-auto flex-shrink-0 scrollbar-none">
        <Lightbulb className="w-4 h-4 text-amber-400 flex-shrink-0 ml-1" />
        <span className="text-xs font-bold text-slate-400 uppercase tracking-wider flex-shrink-0 mr-1">Suggested:</span>
        {quickPrompts.map((qp, i) => (
          <button
            key={i}
            onClick={() => handleSend(qp)}
            className="px-3.5 py-1.5 bg-slate-800/80 hover:bg-slate-700 text-slate-200 rounded-xl text-xs font-medium border border-white/5 flex-shrink-0 transition-all hover:scale-[1.02] active:scale-[0.98] shadow-sm cursor-pointer"
          >
            {qp}
          </button>
        ))}
      </div>

      {/* Message History Feed */}
      <div className="flex-1 p-6 overflow-y-auto space-y-6 bg-slate-950/40">
        {messages.map((m) => (
          <div key={m.id} className={`flex items-start gap-4 ${m.sender === 'user' ? 'flex-row-reverse' : ''}`}>
            <div className={`avatar avatar-md flex-shrink-0 ${m.sender === 'user' ? 'bg-teal-600' : 'bg-orange-600'} text-white font-bold shadow-md`}>
              {m.sender === 'user' ? <User className="w-5 h-5" /> : <Bot className="w-5 h-5" />}
            </div>
            <div className={`p-5 rounded-3xl max-w-[85%] md:max-w-[75%] shadow-xl ${
              m.sender === 'user' ? 'bg-teal-600 text-white rounded-tr-none' : 'bg-slate-800/90 border border-white/10 text-slate-200 rounded-tl-none backdrop-blur-md'
            }`}>
              <div className="text-sm leading-relaxed">
                {m.sender === 'user' ? m.text : formatText(m.text)}
              </div>
              <div className={`text-[10px] mt-3 font-medium flex items-center gap-1 ${m.sender === 'user' ? 'text-teal-100 justify-end' : 'text-slate-400'}`}>
                {m.timestamp}
              </div>
            </div>
          </div>
        ))}
        {loading && (
          <div className="flex items-start gap-4">
            <div className="avatar avatar-md bg-orange-600 text-white shadow-md flex-shrink-0"><Bot className="w-5 h-5" /></div>
            <div className="p-4 bg-slate-800/90 border border-white/10 rounded-3xl rounded-tl-none text-slate-400 text-xs font-medium flex items-center gap-2 shadow-xl">
              <span className="w-2 h-2 bg-orange-400 rounded-full animate-bounce" />
              <span className="w-2 h-2 bg-orange-400 rounded-full animate-bounce delay-100" />
              <span className="w-2 h-2 bg-orange-400 rounded-full animate-bounce delay-200" />
              <span className="ml-1 text-slate-300 font-semibold">Copilot is analyzing enterprise policies...</span>
            </div>
          </div>
        )}
        <div ref={messagesEndRef} />
      </div>

      {/* Input Form Bar */}
      <form onSubmit={(e) => { e.preventDefault(); handleSend(); }} className="p-4 bg-slate-950 border-t border-white/10 flex items-center gap-3 flex-shrink-0">
        <input
          type="text"
          className="form-control flex-1 text-sm py-3 px-4 bg-slate-900 border-white/10 text-white placeholder-slate-500 rounded-2xl focus:border-teal-500/50 focus:outline-none focus:ring-1 focus:ring-teal-500/50"
          placeholder="Ask Copilot anything about policies, leaves, payslips, or performance reviews..."
          value={inputMsg}
          onChange={(e) => setInputMsg(e.target.value)}
        />
        <button 
          type="submit" 
          className="px-6 py-3 bg-teal-600 hover:bg-teal-500 text-white font-bold rounded-2xl flex items-center gap-2 flex-shrink-0 shadow-lg shadow-teal-500/25 transition-all cursor-pointer disabled:opacity-50" 
          disabled={loading || !inputMsg.trim()}
        >
          <span>Send</span>
          <Send className="w-4 h-4" />
        </button>
      </form>
    </div>
  );
}
