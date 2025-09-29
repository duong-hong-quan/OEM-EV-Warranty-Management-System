import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import Dashboard from "./pages/Dashboard";
import VehicleRegistration from "./pages/VehicleRegistration";
import PartAttachment from "./pages/PartAttachment";
import ServiceHistory from "./pages/ServiceHistory";
import WarrantyClaim from "./pages/WarrantyClaim";
import AdminLayout from "./components/AdminLayout";
import './App.css';
import Login from "./pages/Login";
import React, { useEffect } from "react";

function App() {
  useEffect(() => {
    const handler = (e) => {
      const msg = (e && e.detail && e.detail.message) || "Unauthorized";
      try {
        const node = document.createElement("div");
        node.className = "fixed top-4 right-4 z-50 bg-red-600 text-white px-4 py-2 rounded shadow";
        node.textContent = msg;
        document.body.appendChild(node);
        setTimeout(() => node.remove(), 3000);
      } catch {}
    };
    window.addEventListener("app:unauthorized", handler);
    return () => window.removeEventListener("app:unauthorized", handler);
  }, []);

  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/" element={<AdminLayout><Dashboard /></AdminLayout>} />
        <Route path="/dashboard" element={<Navigate to="/" replace />} />
        <Route path="/vehicle-registration" element={<AdminLayout><VehicleRegistration /></AdminLayout>} />
        <Route path="/part-attachment" element={<AdminLayout><PartAttachment /></AdminLayout>} />
        <Route path="/service-history" element={<AdminLayout><ServiceHistory /></AdminLayout>} />
        <Route path="/warranty-claim" element={<AdminLayout><WarrantyClaim /></AdminLayout>} />
        {/* Catch all unmatched routes and redirect to dashboard */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </Router>
  );
}

export default App;
