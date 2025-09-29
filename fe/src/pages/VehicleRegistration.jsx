import React, { useState, useEffect } from "react"
import { api } from "@/lib/api"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"
import { Alert, AlertDescription } from "@/components/ui/alert"
import { ScrollArea } from "@/components/ui/scroll-area"
import { Loader2 } from "lucide-react"
import { Table, TableHeader, TableRow, TableHead, TableBody, TableCell } from "@/components/ui/table";

export default function VehicleRegistration() {
  const [vin, setVin] = useState("")
  const [vehicleName, setVehicleName] = useState("")
  const [name, setName] = useState("")
  const [phone, setPhone] = useState("")
  const [email, setEmail] = useState("")
  const [address, setAddress] = useState("")
  const [status, setStatus] = useState(null)
  const [vehicles, setVehicles] = useState([])
  const [search, setSearch] = useState("")
  const [loading, setLoading] = useState(false)
  const [submitting, setSubmitting] = useState(false)

  useEffect(() => {
    fetchVehicles()
  }, [])

  const fetchVehicles = async () => {
    setLoading(true)
    try {
      const res = await api.get("/api/vehicles")
      const data = res.data
      const items = Array.isArray(data) ? data : (data?.items || [])
      setVehicles(items)
    } catch {
      setStatus({ type: "error", message: "Failed to fetch vehicles." })
    }
    setLoading(false)
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setSubmitting(true)
    setStatus(null)
    try {
      const customerRes = await api.post("/api/customers", { Name: name, Phone: phone, Email: email, Address: address })
      const customerId = customerRes.data?.id || customerRes.data?.Id
      await api.post("/api/vehicles", { VIN: vin, VehicleName: vehicleName, CustomerId: customerId })
      setStatus({ type: "success", message: "Vehicle registered successfully!" })
      setVin(""); setVehicleName(""); setName(""); setPhone(""); setEmail(""); setAddress("")
      fetchVehicles()
    } catch {
      setStatus({ type: "error", message: "Error registering vehicle." })
    }
    setSubmitting(false)
  }

  const filteredVehicles = vehicles.filter(v =>
    (v.vin || v.VIN || "").toLowerCase().includes(search.toLowerCase()) ||
    (v.vehicleName || v.VehicleName || "").toLowerCase().includes(search.toLowerCase()) ||
    (((v.customer && (v.customer.name || v.customer.Name)) || "").toLowerCase().includes(search.toLowerCase()))
  )

  return (
    <div className="p-6 space-y-6">
      {/* Form */}
      <Card className="shadow-sm border border-slate-200">
        <CardHeader>
          <CardTitle className="text-slate-800">Register Vehicle by VIN</CardTitle>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit} className="grid gap-4 md:grid-cols-2">
            <div>
              <Label htmlFor="vin">VIN</Label>
              <Input id="vin" value={vin} onChange={e => setVin(e.target.value)} placeholder="Enter VIN" />
            </div>
            <div>
              <Label htmlFor="vehicleName">Vehicle Name</Label>
              <Input id="vehicleName" value={vehicleName} onChange={e => setVehicleName(e.target.value)} placeholder="Enter vehicle name" />
            </div>
            <div>
              <Label htmlFor="name">Customer Name</Label>
              <Input id="name" value={name} onChange={e => setName(e.target.value)} placeholder="Enter name" />
            </div>
            <div>
              <Label htmlFor="phone">Phone</Label>
              <Input id="phone" value={phone} onChange={e => setPhone(e.target.value)} placeholder="Enter phone" />
            </div>
            <div>
              <Label htmlFor="email">Email</Label>
              <Input id="email" value={email} onChange={e => setEmail(e.target.value)} placeholder="Enter email" />
            </div>
            <div className="md:col-span-2">
              <Label htmlFor="address">Address</Label>
              <Input id="address" value={address} onChange={e => setAddress(e.target.value)} placeholder="Enter address" />
            </div>
            <div className="md:col-span-2 flex justify-end">
              <Button type="submit" disabled={submitting}>
                {submitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                Register Vehicle
              </Button>
            </div>
          </form>

          {status && (
            <Alert className="mt-4" variant={status.type === "success" ? "default" : "destructive"}>
              <AlertDescription>{status.message}</AlertDescription>
            </Alert>
          )}
        </CardContent>
      </Card>

      {/* Search + Table */}
      <Card className="shadow-sm border border-slate-200">
        <CardHeader>
          <CardTitle className="text-slate-800">Registered Vehicles</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="mb-4 flex items-center gap-2">
            <Input
              value={search}
              onChange={e => setSearch(e.target.value)}
              placeholder="Search by VIN, vehicle name or customer name"
              className="max-w-sm"
            />
          </div>
          {loading ? (
            <div className="flex items-center text-gray-500">
              <Loader2 className="mr-2 h-4 w-4 animate-spin" />
              Loading vehicles...
            </div>
          ) : (
            <ScrollArea className="h-[400px] rounded-md border">
              <Table>
                <TableHeader className="bg-gray-50">
                  <TableRow>
                    <TableHead className="px-3 py-2 text-xs font-semibold uppercase text-gray-500 whitespace-nowrap">VIN</TableHead>
                    <TableHead className="px-3 py-2 text-xs font-semibold uppercase text-gray-500">Vehicle Name</TableHead>
                    <TableHead className="px-3 py-2 text-xs font-semibold uppercase text-gray-500">Customer Name</TableHead>
                    <TableHead className="px-3 py-2 text-xs font-semibold uppercase text-gray-500">Customer Email</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {filteredVehicles.map((row, idx) => (
                    <TableRow key={row.id || row.Id} className={idx % 2 === 0 ? "bg-white" : "bg-gray-50/60"}>
                      <TableCell className="px-3 py-2 font-mono whitespace-nowrap">{row.vin || row.VIN}</TableCell>
                      <TableCell className="px-3 py-2">{row.vehicleName || row.VehicleName}</TableCell>
                      <TableCell className="px-3 py-2">{(row.customer && (row.customer.name || row.customer.Name)) || ""}</TableCell>
                      <TableCell className="px-3 py-2 whitespace-nowrap">
                        <span className="inline-block max-w-[220px] truncate align-middle">{(row.customer && (row.customer.email || row.customer.Email)) || ""}</span>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </ScrollArea>
          )}
        </CardContent>
      </Card>
    </div>
  )
}
