'use client';

import React, { useState } from 'react';
import { Award, ThumbsUp, MessageSquare, Plus, Check } from 'lucide-react';
import { useRole } from '../context/RoleContext';

export default function RecognitionFeature() {
  const { activeRole, graphqlFetch, token } = useRole();
  const [showModal, setShowModal] = useState(false);
  const [recognitions, setRecognitions] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const loadRecognitions = async () => {
    if (!token) return;
    setLoading(true);
    try {
      const query = `
        query {
          recentRecognitions {
            id
            giverName
            receiverName
            receiverEmail
            category
            message
            points
            awardedDate
          }
        }
      `;
      const data = await graphqlFetch(query);
      if (data && data.recentRecognitions) {
        setRecognitions(data.recentRecognitions.map(rec => ({
          id: rec.id,
          sender: rec.giverName,
          senderAvatar: rec.giverName.split(' ').map(n => n[0]).join(''),
          recipient: rec.receiverName,
          recipientAvatar: rec.receiverName.split(' ').map(n => n[0]).join(''),
          category: rec.category,
          message: rec.message,
          visibility: 'public',
          likes: 0,
          comments: 0,
          timestamp: new Date(rec.awardedDate).toLocaleDateString(),
          liked: false
        })));
      }
    } catch (err) {
      setError(err.message || 'Failed to load recognitions');
    } finally {
      setLoading(false);
    }
  };

  React.useEffect(() => {
    loadRecognitions();
  }, [token]);

  const colleagues = ['Michael Chen', 'Emma Rodriguez', 'Tom Harris', 'Arjun Mehta', 'Priya Nair', 'Rachel Green', 'Sneha Rao'];

  // Form state
  const [recipient, setRecipient] = useState(colleagues[0]);
  const [category, setCategory] = useState('excellence');
  const [message, setMessage] = useState('');
  const [visibility, setVisibility] = useState('public');

  const handleToggleLike = (id) => {
    setRecognitions(prev => prev.map(rec => {
      if (rec.id === id) {
        return { ...rec, liked: !rec.liked, likes: rec.liked ? rec.likes - 1 : rec.likes + 1 };
      }
      return rec;
    }));
  };

  const handleSendRecognition = async (e) => {
    e.preventDefault();
    if (!message) return;

    try {
      const mutation = `
        mutation SendRecognition($command: SendRecognitionCommandInput!) {
          sendRecognition(command: $command) {
            id
          }
        }
      `;
      const variables = {
        command: {
          receiverName: recipient,
          receiverEmail: `${recipient.split(' ').join('.').toLowerCase()}@hrms.com`,
          category,
          message,
          points: 10
        }
      };
      const res = await graphqlFetch(mutation, variables);
      if (res && res.sendRecognition) {
        setShowModal(false);
        setMessage('');
        await loadRecognitions();
      }
    } catch (err) {
      alert(`Error sending recognition: ${err.message}`);
    }
  };

  const getBadgeClass = (cat) => {
    switch(cat) {
      case 'excellence': return 'badge-orange';
      case 'team-player': return 'badge-teal';
      case 'innovation': return 'badge-purple';
      case 'leadership': return 'badge-blue';
      case 'customer-focus': return 'badge-success';
      default: return 'badge-slate';
    }
  };

  return (
    <div className="animate-fade-in space-y-6">
      <div className="section-header">
        <div>
          <h1 className="section-title"><Award className="w-7 h-7 text-teal-500" /> Peer Recognition Feed</h1>
          <p className="section-subtitle">Company-wide social praise feed celebrating core values and exceptional teamwork</p>
        </div>
        <button className="btn-teal py-3 px-5 text-sm" onClick={() => setShowModal(true)}>
          <Plus className="w-4 h-4" /> Send Recognition
        </button>
      </div>

      {/* Recognition Feed */}
      {error && (
        <div className="p-4 bg-rose-500/10 border border-rose-500/20 text-rose-400 text-sm rounded-xl">
          {error}
        </div>
      )}
      {loading ? (
        <div className="p-12 text-center text-slate-400">Loading social feed...</div>
      ) : (
        <div className="max-w-3xl mx-auto space-y-6">
          {recognitions.length === 0 ? (
            <div className="p-8 text-center text-slate-500 italic">No recognitions found in the feed.</div>
          ) : recognitions.map((rec) => (
          <div key={rec.id} className="glass-card space-y-4">
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-3 border-b border-white/10 pb-4">
              <div className="flex items-center gap-3">
                <div className="avatar avatar-md bg-teal-600 text-white font-bold">{rec.senderAvatar}</div>
                <div className="font-bold text-white text-sm">{rec.sender}</div>
                <span className="text-slate-500 text-xs">&rarr; recognized</span>
                <div className="avatar avatar-md bg-orange-600 text-white font-bold">{rec.recipientAvatar}</div>
                <div className="font-bold text-white text-sm">{rec.recipient}</div>
              </div>
              <div className="flex items-center gap-3">
                <span className={`badge ${getBadgeClass(rec.category)} uppercase text-[10px]`}>{rec.category.replace('-', ' ')}</span>
                <span className="text-xs text-slate-500">{rec.timestamp}</span>
              </div>
            </div>

            <p className="text-base text-slate-200 leading-relaxed italic pl-4 border-l-2 border-white/20 py-2">
              "{rec.message}"
            </p>

            <div className="border-t border-white/10 pt-4 flex items-center justify-between text-xs text-slate-400">
              <div className="flex items-center gap-4">
                <button
                  className={`inline-flex items-center gap-1.5 py-1 px-3 rounded-xl border transition-colors ${
                    rec.liked ? 'bg-teal-500/20 border-teal-500/40 text-teal-300' : 'bg-slate-900/50 border-white/10 hover:bg-slate-900 text-slate-400'
                  }`}
                  onClick={() => handleToggleLike(rec.id)}
                >
                  <ThumbsUp className="w-3.5 h-3.5" /> {rec.likes} {rec.likes === 1 ? 'Like' : 'Likes'}
                </button>
                <span className="inline-flex items-center gap-1.5 py-1 px-3 rounded-xl bg-slate-900/50 border border-white/10">
                  <MessageSquare className="w-3.5 h-3.5" /> {rec.comments} Comments
                </span>
              </div>
              <span className="badge badge-slate uppercase text-[9px]">{rec.visibility}</span>
            </div>
          </div>
          ))}
        </div>
      )}
      {/* Send Recognition Modal */}
      {showModal && (
        <div className="modal-backdrop">
          <div className="modal-panel max-w-lg">
            <div className="modal-header">
              <h3 className="text-lg font-bold text-white">Recognize a Colleague</h3>
              <button onClick={() => setShowModal(false)} className="text-slate-400 hover:text-white">&times;</button>
            </div>
            <form onSubmit={handleSendRecognition}>
              <div className="modal-body space-y-4">
                <div className="form-group">
                  <label className="form-label">Recipient</label>
                  <select className="form-control" value={recipient} onChange={(e) => setRecipient(e.target.value)}>
                    {colleagues.map(coll => <option key={coll} value={coll} className="bg-slate-900">{coll}</option>)}
                  </select>
                </div>
                <div className="form-group">
                  <label className="form-label">Value Category</label>
                  <select className="form-control" value={category} onChange={(e) => setCategory(e.target.value)}>
                    <option value="excellence" className="bg-slate-900">🌟 Excellence</option>
                    <option value="team-player" className="bg-slate-900">🤝 Team Player</option>
                    <option value="innovation" className="bg-slate-900">💡 Innovation</option>
                    <option value="leadership" className="bg-slate-900">👑 Leadership</option>
                    <option value="customer-focus" className="bg-slate-900">🎯 Customer Focus</option>
                  </select>
                </div>
                <div className="form-group">
                  <label className="form-label">Recognition Message</label>
                  <textarea className="form-control" rows="4" placeholder="Be specific about what they did and the impact it had..." value={message} onChange={(e) => setMessage(e.target.value)} required></textarea>
                </div>
                <div className="form-group mb-0">
                  <label className="form-label">Visibility</label>
                  <select className="form-control" value={visibility} onChange={(e) => setVisibility(e.target.value)}>
                    <option value="public" className="bg-slate-900">Public (Company Feed)</option>
                    <option value="private" className="bg-slate-900">Private (Manager & Recipient Only)</option>
                  </select>
                </div>
              </div>
              <div className="modal-footer">
                <button type="button" className="btn-ghost" onClick={() => setShowModal(false)}>Cancel</button>
                <button type="submit" className="btn-teal">Share Recognition</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
