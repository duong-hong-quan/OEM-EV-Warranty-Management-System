import React from "react";
import { Link, useLocation } from "react-router-dom";
import { cn } from "@/lib/utils";
import { Home, Car, Settings, History, FileText } from "lucide-react";

export default function AdminLayout({ children }) {
  const location = useLocation();

  const navItems = [
    { to: "/", label: "Dashboard", icon: <Home className="h-4 w-4" /> },
    {
      to: "/vehicle-registration",
      label: "Vehicle Registration",
      icon: <Car className="h-4 w-4" />,
    },
    {
      to: "/part-attachment",
      label: "Part Attachment",
      icon: <Settings className="h-4 w-4" />,
    },
    {
      to: "/service-history",
      label: "Service History",
      icon: <History className="h-4 w-4" />,
    },
    {
      to: "/warranty-claim",
      label: "Warranty Claim",
      icon: <FileText className="h-4 w-4" />,
    },
  ];

  return (
    <div className="min-h-screen bg-gray-100">
      {/* Sidebar (fixed) */}
      <aside className="fixed left-0 top-0 h-screen w-60 bg-white border-r flex flex-col">
        <div className="p-6 border-b">
          <h2 className="text-lg font-semibold">EV Warranty Admin</h2>
        </div>
        <nav className="flex-1 p-4 flex flex-col gap-1">
          {navItems.map((item) => (
            <Link
              key={item.to}
              to={item.to}
              className={cn(
                "flex items-center gap-2 rounded-md px-3 py-2 text-sm font-medium transition-colors",
                location.pathname === item.to
                  ? "bg-blue-100 text-blue-700"
                  : "text-gray-700 hover:bg-gray-100"
              )}
            >
              {item.icon}
              {item.label}
            </Link>
          ))}
        </nav>
        <div className="p-4 border-t text-xs text-gray-500">
          © 2025 EV Warranty
        </div>
      </aside>

      {/* Main Content (scrollable) */}
      <main className="ml-60 p-6 h-screen overflow-y-auto">{children}</main>
    </div>
  );
}
