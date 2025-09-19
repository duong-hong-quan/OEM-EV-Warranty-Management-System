import React from "react";

export default function StatCard({ title, value, icon }) {
  return (
    <div className="flex items-center gap-4 bg-white rounded-lg shadow p-6 min-w-[180px]">
      <div className="text-blue-600 text-3xl">{icon}</div>
      <div>
        <div className="text-lg font-semibold">{title}</div>
        <div className="text-2xl font-bold">{value}</div>
      </div>
    </div>
  );
}
