import React, { useEffect, useMemo, useState } from "react"
import { api } from "@/lib/api"
import { Input } from "@/components/ui/input"
import { ScrollArea } from "@/components/ui/scroll-area"
import { Loader2, Car } from "lucide-react"

export default function VehicleSelect({ value, onChange, placeholder = "Search VIN, vehicle name, or customer", disabled = false }) {
  const [vehicles, setVehicles] = useState([])
  const [loading, setLoading] = useState(false)
  const [query, setQuery] = useState("")

  useEffect(() => {
    const load = async () => {
      setLoading(true)
      try {
        const res = await api.get("/api/vehicles")
        const data = res.data
        const items = Array.isArray(data) ? data : (data?.items || [])
        setVehicles(items)
      } finally {
        setLoading(false)
      }
    }
    load()
  }, [])

  const filtered = useMemo(() => {
    const q = query.trim().toLowerCase()
    if (!q) return vehicles
    return vehicles.filter(v => {
      const vin = (v.vin || v.VIN || "").toLowerCase()
      const name = (v.vehicleName || v.VehicleName || "").toLowerCase()
      const customer = (((v.customer && (v.customer.name || v.customer.Name)) || "")).toLowerCase()
      return vin.includes(q) || name.includes(q) || customer.includes(q)
    })
  }, [vehicles, query])

  const current = useMemo(() => {
    if (!value) return null
    return vehicles.find(v => (v.id || v.Id) === value) || null
  }, [vehicles, value])

  return (
    <div className="space-y-2">
      <div className="relative">
        <Input
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          placeholder={placeholder}
          disabled={disabled}
        />
        <Car className="absolute right-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400" />
      </div>
      <div className="text-xs text-gray-500">
        {current ? (
          <span>
            Selected: {(current.vin || current.VIN)}
            {current.vehicleName || current.VehicleName ? ` • ${(current.vehicleName || current.VehicleName)}` : ""}
          </span>
        ) : (
          <span>No vehicle selected</span>
        )}
      </div>
      <div className="border rounded-md">
        {loading ? (
          <div className="flex items-center gap-2 p-3 text-gray-500">
            <Loader2 className="h-4 w-4 animate-spin" /> Loading vehicles...
          </div>
        ) : (
          <ScrollArea className="h-48">
            <ul>
              {filtered.map(v => {
                const id = v.id || v.Id
                const vin = v.vin || v.VIN
                const name = v.vehicleName || v.VehicleName
                const customer = v.customer?.name || v.customer?.Name || ""
                const selected = value === id
                return (
                  <li key={id}>
                    <button
                      type="button"
                      onClick={() => onChange?.(id)}
                      className={`w-full text-left px-3 py-2 text-sm hover:bg-gray-50 ${selected ? "bg-blue-50" : ""}`}
                    >
                      <div className="font-mono text-[12px] text-gray-700">{vin}</div>
                      <div className="text-gray-600">
                        {name}
                        {customer ? <span className="text-gray-400"> • {customer}</span> : null}
                      </div>
                    </button>
                  </li>
                )
              })}
              {filtered.length === 0 && (
                <li className="px-3 py-2 text-sm text-gray-500">No results</li>
              )}
            </ul>
          </ScrollArea>
        )}
      </div>
    </div>
  )
} 