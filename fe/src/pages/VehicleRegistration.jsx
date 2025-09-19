import React, { useState, useEffect } from "react"
import axios from "axios"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"
import { Alert, AlertDescription } from "@/components/ui/alert"
import { ScrollArea } from "@/components/ui/scroll-area"
import { Loader2 } from "lucide-react"
import { Table } from "@/components/ui/table";

export default function VehicleRegistration() {
  const [vin, setVin] = useState("")
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
      const res = await axios.get("http://localhost:5165/api/Vehicles")
      setVehicles(res.data)
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
      const customerRes = await axios.post("http://localhost:5165/api/Customers", { name, phone, email, address })
      await axios.post("http://localhost:5165/api/Vehicles", { vin, customerId: customerRes.data.id })
      setStatus({ type: "success", message: "Vehicle registered successfully!" })
      setVin(""); setName(""); setPhone(""); setEmail(""); setAddress("")
      fetchVehicles()
    } catch {
      setStatus({ type: "error", message: "Error registering vehicle." })
    }
    setSubmitting(false)
  }

  const filteredVehicles = vehicles.filter(v =>
    v.vin.toLowerCase().includes(search.toLowerCase()) ||
    (v.customerName && v.customerName.toLowerCase().includes(search.toLowerCase()))
  )

  const columns = [
    { Header: "VIN", accessor: "vin" },
    { Header: "Customer Name", accessor: "customerName" },
    { Header: "Phone", accessor: "customerPhone" },
    { Header: "Email", accessor: "customerEmail" },
    { Header: "Address", accessor: "customerAddress" }
  ]

  return (
    <div className="p-6 space-y-6">
      {/* Form */}
      <Card>
        <CardHeader>
          <CardTitle>Register Vehicle by VIN</CardTitle>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit} className="grid gap-4 md:grid-cols-2">
            <div>
              <Label htmlFor="vin">VIN</Label>
              <Input id="vin" value={vin} onChange={e => setVin(e.target.value)} placeholder="Enter VIN" />
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
      <Card>
        <CardHeader>
          <CardTitle>Registered Vehicles</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="mb-4 flex items-center gap-2">
            <Input
              value={search}
              onChange={e => setSearch(e.target.value)}
              placeholder="Search by VIN or customer name"
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
              <Table columns={columns} data={filteredVehicles} />
            </ScrollArea>
          )}
        </CardContent>
      </Card>
    </div>
  )
}
