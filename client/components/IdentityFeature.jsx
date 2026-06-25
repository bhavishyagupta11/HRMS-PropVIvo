import React from 'react';

export default function IdentityFeature() {
  return (
    <div className="animate-fade-in">
      <div className="glass-card" style={{ maxWidth: '600px', margin: '0 auto' }}>
        <h2 className="card-title">Identity & Security Context</h2>
        <div className="form-group">
          <label className="form-label">Authenticated User ID</label>
          <div className="form-control" style={{ background: 'rgba(255,255,255,0.05)' }}>USR-9988231-HRMS</div>
        </div>
        <div className="form-group">
          <label className="form-label">Email Address</label>
          <div className="form-control" style={{ background: 'rgba(255,255,255,0.05)' }}>employee.premier@enterprise.hrms</div>
        </div>
        <div className="form-group">
          <label className="form-label">Security Role</label>
          <div style={{ marginTop: '0.5rem' }}>
            <span className="status-tag status-success">Global Enterprise User</span>
          </div>
        </div>
        <div className="form-group" style={{ marginTop: '2rem' }}>
          <button className="btn btn-primary" style={{ width: '100%' }} onClick={() => alert('Token refreshed successfully!')}>
            Refresh JWT Access Token
          </button>
        </div>
      </div>
    </div>
  );
}
