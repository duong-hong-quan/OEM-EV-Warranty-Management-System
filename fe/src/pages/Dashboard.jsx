import React from "react";
import StatCard from "@/components/StatCard";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { CheckCircle2 } from "lucide-react";

export default function Dashboard() {
  // Demo stats
  const stats = [
    {
      title: "Vehicles",
      value: 128,
      color: "from-blue-500 to-blue-600",
    },
    {
      title: "Customers",
      value: 97,
      color: "from-green-500 to-green-600",
    },
    {
      title: "Parts Attached",
      value: 312,
      color: "from-yellow-500 to-yellow-600",
    },
    {
      title: "Warranty Claims",
      value: 14,
      color: "from-red-500 to-red-600",
    },
  ];

  const features = [
    "Vehicle & Customer Registration",
    "Part Serial Attachment",
    "Service & Warranty History",
    "Warranty Claim Management",
  ];

  return (
    <div className="p-8 bg-gray-50 min-h-screen">
      {/* Header */}
      <Card className="mb-8 bg-gradient-to-r from-blue-600 to-indigo-600 text-white">
        <CardHeader>
          <CardTitle className="text-2xl font-bold">
            EV Warranty Management Dashboard
          </CardTitle>
        </CardHeader>
        <CardContent>
          <p className="text-blue-100">
            Monitor your EV ecosystem with vehicles, parts, services, and
            warranty claims — all in one place.
          </p>
        </CardContent>
      </Card>

      {/* Stats */}
      <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-4 mb-10">
        {stats.map((s, i) => (
          <Card
            key={i}
            className={`bg-gradient-to-r ${s.color} text-white shadow-md`}
          >
            <CardContent className="p-6 flex items-center justify-between">
              <div>
                <p className="text-sm opacity-80">{s.title}</p>
                <p className="text-3xl font-bold">{s.value}</p>
              </div>
              <div className="text-4xl">{s.icon}</div>
            </CardContent>
          </Card>
        ))}
      </div>

      {/* Features */}
      <Card className="shadow-sm">
        <CardHeader>
          <CardTitle>Features</CardTitle>
        </CardHeader>
        <CardContent>
          <ul className="grid grid-cols-1 md:grid-cols-2 gap-4 text-gray-700">
            {features.map((f, idx) => (
              <li
                key={idx}
                className="flex items-center gap-2 p-3 rounded-md hover:bg-gray-50 transition"
              >
                <CheckCircle2 className="h-5 w-5 text-blue-500" />
                <span className="text-base">{f}</span>
              </li>
            ))}
          </ul>
        </CardContent>
      </Card>
    </div>
  );
}
