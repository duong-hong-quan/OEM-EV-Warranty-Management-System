import React, { useState, useEffect } from "react"
import { api } from "@/lib/api"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Textarea } from "@/components/ui/textarea"
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Loader2 } from "lucide-react"
import VehicleSelect from "@/components/VehicleSelect"

export default function WarrantyClaim() {
  const [vehicleId, setVehicleId] = useState("")
  const [report, setReport] = useState("")
  const [diagnostic, setDiagnostic] = useState("")
  const [images, setImages] = useState([])
  const [status, setStatus] = useState("")
  const [claims, setClaims] = useState([])
  const [loading, setLoading] = useState(false)

  useEffect(() => {
    api.get("/api/warranty-claim")
      .then((res) => setClaims(res.data?.items || res.data || []))
      .catch(() => setStatus("Failed to load claims."))
  }, [])

  const handleSubmit = async (e) => {
    e.preventDefault()
    setStatus("")
    setLoading(true)
    try {
      const res = await api.post("/api/warranty-claim", {
        VehicleId: vehicleId,
        Report: report,
        DiagnosticInfo: diagnostic,
        ImageUrls: [],
      })

      if (res.status === 201 || res.status === 200) {
        setStatus("✅ Claim submitted successfully!")
        setVehicleId("")
        setReport("")
        setDiagnostic("")
        setImages([])
        setClaims((prev) => [...prev, res.data])
      } else {
        setStatus("❌ Failed to submit claim.")
      }
    } catch {
      setStatus("⚠ Error connecting to server.")
    } finally {
      setLoading(false)
    }
  }

  const getBadgeColor = (status) => {
    switch (status) {
      case "Approved":
        return "bg-green-100 text-green-700"
      case "Rejected":
        return "bg-red-100 text-red-700"
      default:
        return "bg-yellow-100 text-yellow-700"
    }
  }

  return (
    <div className="p-6 space-y-8">
      {/* Create Claim Form */}
      <Card className="shadow-sm border border-slate-200">
        <CardHeader>
          <CardTitle className="text-slate-800">Create Warranty Claim</CardTitle>
        </CardHeader>
        <CardContent>
          <form className="space-y-4" onSubmit={handleSubmit}>
            <div>
              <Label>Vehicle</Label>
              <VehicleSelect value={vehicleId} onChange={setVehicleId} placeholder="Search vehicles..." />
            </div>
            <div>
              <Label htmlFor="report">Report</Label>
              <Textarea
                id="report"
                value={report}
                onChange={(e) => setReport(e.target.value)}
                placeholder="Enter inspection report"
                rows={3}
                required
              />
            </div>
            <div>
              <Label htmlFor="diagnostic">Diagnostic Info</Label>
              <Textarea
                id="diagnostic"
                value={diagnostic}
                onChange={(e) => setDiagnostic(e.target.value)}
                placeholder="Enter diagnostic info"
                rows={3}
              />
            </div>
            <div>
              <Label htmlFor="images">Upload Images</Label>
              <Input
                id="images"
                type="file"
                multiple
                onChange={(e) =>
                  setImages(Array.from(e.target.files || []))
                }
              />
              {images.length > 0 && (
                <div className="flex gap-2 mt-2 flex-wrap">
                  {images.map((file, i) => (
                    <span
                      key={i}
                      className="text-xs bg-gray-100 px-2 py-1 rounded"
                    >
                      {file.name}
                    </span>
                  ))}
                </div>
              )}
            </div>
            <Button type="submit" disabled={loading || !vehicleId}>
              {loading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
              Submit Claim
            </Button>
          </form>
          {status && <div className="mt-4 text-sm font-medium">{status}</div>}
        </CardContent>
      </Card>

      {/* Claims Table */}
      <Card className="shadow-sm border border-slate-200">
        <CardHeader>
          <CardTitle className="text-slate-800">Claim Status Tracking</CardTitle>
        </CardHeader>
        <CardContent>
          {claims.length === 0 ? (
            <p className="text-gray-500">No claims submitted yet.</p>
          ) : (
            <div className="overflow-x-auto">
              <table className="min-w-full border text-sm">
                <thead className="bg-slate-50 text-left">
                  <tr>
                    <th className="border px-3 py-2 text-xs font-semibold uppercase text-slate-500">Date</th>
                    <th className="border px-3 py-2 text-xs font-semibold uppercase text-slate-500">Status</th>
                    <th className="border px-3 py-2 text-xs font-semibold uppercase text-slate-500">Report</th>
                  </tr>
                </thead>
                <tbody>
                  {claims.map((c, idx) => (
                    <tr
                      key={c.id}
                      className={idx % 2 === 0 ? "bg-white" : "bg-slate-50/60"}
                    >
                      <td className="border px-3 py-2">
                        {new Date(c.claimDate).toLocaleDateString()}
                      </td>
                      <td className="border px-3 py-2">
                        <Badge className={getBadgeColor(c.status)}>
                          {c.status}
                        </Badge>
                      </td>
                      <td className="border px-3 py-2">{c.report}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  )
}
