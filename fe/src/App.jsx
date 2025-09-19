import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Dashboard from "./pages/Dashboard";
import VehicleRegistration from "./pages/VehicleRegistration";
import PartAttachment from "./pages/PartAttachment";
import ServiceHistory from "./pages/ServiceHistory";
import WarrantyClaim from "./pages/WarrantyClaim";
import AdminLayout from "./components/AdminLayout";
import './App.css';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<AdminLayout><Dashboard /></AdminLayout>} />
        <Route path="/vehicle-registration" element={<AdminLayout><VehicleRegistration /></AdminLayout>} />
        <Route path="/part-attachment" element={<AdminLayout><PartAttachment /></AdminLayout>} />
        <Route path="/service-history" element={<AdminLayout><ServiceHistory /></AdminLayout>} />
        <Route path="/warranty-claim" element={<AdminLayout><WarrantyClaim /></AdminLayout>} />
      </Routes>
    </Router>
  );
}

export default App;
