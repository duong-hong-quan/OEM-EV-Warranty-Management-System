import React, { useEffect, useState } from "react";
import StatCard from "@/components/StatCard";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { CheckCircle2 } from "lucide-react";
import { api } from "@/lib/api";

export default function Dashboard() {
  const [stats, setStats] = useState([
    { title: "Vehicles", value: 0, color: "from-gray-800 to-gray-900" },
    { title: "Customers", value: 0, color: "from-gray-700 to-gray-800" },
    { title: "Parts Attached", value: 0, color: "from-gray-600 to-gray-700" },
    { title: "Warranty Claims", value: 0, color: "from-gray-500 to-gray-600" },
  ]);

  useEffect(() => {
    const countFromPaged = (res) => {
      const data = res?.data;
      if (!data) return 0;
      if (typeof data.totalItems === "number") return data.totalItems;
      if (Array.isArray(data.items)) return data.items.length;
      if (Array.isArray(data)) return data.length;
      return 0;
    };

    const fetchCounts = async () => {
      try {
        const [vehiclesRes, customersRes, partsRes, claimsRes] = await Promise.allSettled([
          api.get("/api/vehicles"),
          api.get("/api/customers"),
          api.get("/api/parts"),
          api.get("/api/warranty-claim"),
        ]);

        const vehicles = vehiclesRes.status === "fulfilled" ? countFromPaged(vehiclesRes.value) : 0;
        const customers = customersRes.status === "fulfilled" ? countFromPaged(customersRes.value) : 0;
        const parts = partsRes.status === "fulfilled" ? countFromPaged(partsRes.value) : 0;
        const claims = claimsRes.status === "fulfilled" ? countFromPaged(claimsRes.value) : 0;

        setStats((prev) => [
          { ...prev[0], value: vehicles },
          { ...prev[1], value: customers },
          { ...prev[2], value: parts },
          { ...prev[3], value: claims },
        ]);
      } catch {
        // ignore on dashboard; cards will stay at 0
      }
    };

    fetchCounts();
  }, []);

  const features = [
    "Vehicle & Customer Registration",
    "Part Serial Attachment",
    "Service & Warranty History",
    "Warranty Claim Management",
  ];

  return (
    <div className="p-8 min-h-screen">
      {/* Header */}
      <Card className="mb-8 bg-white/60 backdrop-blur shadow-sm border border-slate-200">
        <CardHeader>
          <CardTitle className="text-2xl font-bold bg-gradient-to-r from-gray-800 to-gray-900 bg-clip-text text-transparent">
            EV Warranty Management Dashboard
          </CardTitle>
        </CardHeader>
        <CardContent>
          <p className="text-slate-600">
            Monitor your EV ecosystem with vehicles, parts, services, and
            warranty claims — all in one place.
          </p>
        </CardContent>
      </Card>

      {/* Stats */}
      <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-4 mb-10">
        {stats.map((s, i) => (
          <div key={i} className="rounded-xl shadow-sm overflow-hidden">
            <div className={`bg-gradient-to-r ${s.color} text-white`}>
              <div className="p-6 flex items-center justify-between">
                <div>
                  <p className="text-sm/5 opacity-80">{s.title}</p>
                  <p className="text-4xl font-semibold tracking-tight">{s.value}</p>
                </div>
                <div className="text-4xl" />
              </div>
            </div>
          </div>
        ))}
      </div>

      {/* Features */}
      <Card className="shadow-sm border border-slate-200">
        <CardHeader>
          <CardTitle className="text-slate-800">Features</CardTitle>
        </CardHeader>
        <CardContent>
          <ul className="grid grid-cols-1 md:grid-cols-2 gap-4 text-slate-700">
            {features.map((f, idx) => (
              <li
                key={idx}
                className="flex items-center gap-3 p-3 rounded-md hover:bg-slate-50 transition"
              >
                <span className="inline-flex items-center justify-center h-6 w-6 rounded-full bg-gray-100 text-gray-800">
                  <CheckCircle2 className="h-4 w-4" />
                </span>
                <span className="text-base">{f}</span>
              </li>
            ))}
          </ul>
        </CardContent>
      </Card>
    </div>
  );
}
