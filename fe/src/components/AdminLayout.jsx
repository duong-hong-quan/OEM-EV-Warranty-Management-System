import React from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { cn } from "@/lib/utils";
import { Home, Car, Settings, History, FileText, LogOut, LogIn } from "lucide-react";
import { getCurrentRole, getCurrentUser, clearAuth } from "@/lib/auth";

export default function AdminLayout({ children }) {
  const location = useLocation();
  const navigate = useNavigate();
  const role = getCurrentRole();
  const user = getCurrentUser();

  const allNavItems = [
    { to: "/", label: "Dashboard", icon: <Home className="h-4 w-4" />, roles: ["Admin", "Manager", "Technician", "Customer"] },
    { to: "/vehicle-registration", label: "Vehicle Registration", icon: <Car className="h-4 w-4" />, roles: ["Admin", "Manager"] },
    { to: "/part-attachment", label: "Part Attachment", icon: <Settings className="h-4 w-4" />, roles: ["Admin", "Manager", "Technician"] },
    { to: "/service-history", label: "Service History", icon: <History className="h-4 w-4" />, roles: ["Admin", "Manager", "Technician", "Customer"] },
    { to: "/warranty-claim", label: "Warranty Claim", icon: <FileText className="h-4 w-4" />, roles: ["Admin", "Manager", "Customer"] },
  ];

  const navItems = role ? allNavItems.filter(i => i.roles.includes(role)) : allNavItems;

  const handleLogout = () => {
    clearAuth();
    navigate("/login");
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-50 to-slate-100">
      {/* Sidebar (fixed) */}
      <aside className="fixed left-0 top-0 h-screen w-64 bg-white/95 backdrop-blur border-r flex flex-col shadow-sm">
        <div className="p-6 border-b bg-gradient-to-r from-blue-600 to-indigo-600 text-white">
          <h2 className="text-lg font-semibold tracking-wide">EV Warranty Admin</h2>
          <div className="text-xs text-blue-100 mt-1">
            {user ? `${user.name} • ${role}` : "Guest"}
          </div>
        </div>
        <nav className="flex-1 p-3 flex flex-col gap-1">
          {navItems.map((item) => {
            const active = location.pathname === item.to;
            return (
              <Link
                key={item.to}
                to={item.to}
                className={cn(
                  "group relative flex items-center gap-2 rounded-md px-3 py-2 text-sm font-medium transition-all",
                  active
                    ? "bg-blue-50 text-blue-700 shadow-[inset_2px_0_0_0_theme(colors.blue.600)]"
                    : "text-slate-700 hover:bg-slate-50"
                )}
              >
                <span className={cn("shrink-0", active ? "text-blue-700" : "text-slate-500 group-hover:text-slate-700")}>{item.icon}</span>
                <span className="truncate">{item.label}</span>
              </Link>
            );
          })}
        </nav>
        <div className="p-4 border-t text-xs text-gray-500 flex items-center justify-between bg-white/80">
          <span>© 2025 EV Warranty</span>
          {user ? (
            <button onClick={handleLogout} className="flex items-center gap-1 text-gray-600 hover:text-gray-900 transition-colors">
              <LogOut className="h-3.5 w-3.5" /> Logout
            </button>
          ) : (
            <Link to="/login" className="flex items-center gap-1 text-gray-600 hover:text-gray-900 transition-colors">
              <LogIn className="h-3.5 w-3.5" /> Login
            </Link>
          )}
        </div>
      </aside>

      {/* Main Content (scrollable) */}
      <main className="ml-64 p-8 h-screen overflow-y-auto">
        <div className="mx-auto max-w-7xl">{children}</div>
      </main>
    </div>
  );
}
