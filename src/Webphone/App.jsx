import React, { useState, useEffect } from 'react';
import JsSIP from 'jssip';
import UAConfig from './sip/jssip-config';

export default function App() {
  const [ua, setUa] = useState(null);
  const [session, setSession] = useState(null);
  const [number, setNumber] = useState('');
  const [status, setStatus] = useState('disconnected');

  useEffect(() => {
    const userAgent = new JsSIP.UA(UAConfig);
    userAgent.on('connected', () => setStatus('connected'));
    userAgent.on('registered', () => setStatus('registered'));
    userAgent.on('newRTCSession', e => {
      const sess = e.session;
      setSession(sess);
      sess.on('ended', () => { setSession(null); setStatus('registered'); });
      sess.on('stream', () => setStatus('in call'));
      setStatus('incoming');
    });
    userAgent.start();
    setUa(userAgent);
  }, []);

  const dial = () => {
    fetch('/api/discar', { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify({numeros:[number]}) })
      .then(res => res.json())
      .then(json => alert('Atendido por: ' + json.NumeroAtendido));
  };

  const answer = () => {
    session.answer({ mediaConstraints: { audio: true, video: false } });
  };

  return (
    <div style={{padding:20}}>
      <h1>WebPhone</h1>
      <p>Status: {status}</p>
      <input value={number} onChange={e=>setNumber(e.target.value)} placeholder="Número para discar"/>
      <button onClick={dial} disabled={status !== 'registered'}>Discar</button>
      {session && status === 'incoming' && <button onClick={answer}>Atender</button>}
    </div>
  );
}
