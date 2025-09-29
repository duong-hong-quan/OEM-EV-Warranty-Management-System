import React, { useState, useEffect } from "react"
import { api } from "@/lib/api"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"
import { Alert, AlertDescription } from "@/components/ui/alert"
import { ScrollArea } from "@/components/ui/scroll-area"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { Badge } from "@/components/ui/badge"
import { Loader2, Car, Wrench, FileText, Shield } from "lucide-react"
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
  const [selectedVehicle, setSelectedVehicle] = useState(null)

  useEffect(() => {
    fetchVehicles()
  }, [])

  const fetchVehicles = async () => {
    setLoading(true)
    try {
      const res = await api.get("/api/vehicles")
      const data = res.data
      // API mới trả về PagedResult với Items property
      const items = Array.isArray(data) ? data : (data?.Items || data?.items || [])
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

  const getStatusBadgeColor = (status) => {
    switch (status?.toLowerCase()) {
      case 'sent': return 'bg-blue-100 text-blue-800'
      case 'pending': return 'bg-yellow-100 text-yellow-800'
      case 'accepted': return 'bg-green-100 text-green-800'
      case 'processed': return 'bg-purple-100 text-purple-800'
      default: return 'bg-gray-100 text-gray-800'
    }
  }

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
              <Label htmlFor="vin" className="mb-2">VIN:</Label>
              <Input id="vin" value={vin} onChange={e => setVin(e.target.value)} placeholder="Enter VIN" />
            </div>
            <div>
              <Label htmlFor="vehicleName" className="mb-2">Vehicle Name:</Label>
              <Input id="vehicleName" value={vehicleName} onChange={e => setVehicleName(e.target.value)} placeholder="Enter vehicle name" />
            </div>
            <div>
              <Label htmlFor="name" className="mb-2">Customer Name:</Label>
              <Input id="name" value={name} onChange={e => setName(e.target.value)} placeholder="Enter name" />
            </div>
            <div>
              <Label htmlFor="phone" className="mb-2">Phone:</Label>
              <Input id="phone" value={phone} onChange={e => setPhone(e.target.value)} placeholder="Enter phone" />
            </div>
            <div>
              <Label htmlFor="email" className="mb-2">Email:</Label>
              <Input id="email" value={email} onChange={e => setEmail(e.target.value)} placeholder="Enter email" />
            </div>
            <div className="md:col-span-2">
              <Label htmlFor="address" className="mb-2">Address:</Label>
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
                    <TableHead className="px-3 py-2 text-xs font-semibold uppercase text-gray-500 whitespace-nowrap text-center">VIN</TableHead>
                    <TableHead className="px-3 py-2 text-xs font-semibold uppercase text-gray-500 text-center">Vehicle Name</TableHead>
                    <TableHead className="px-3 py-2 text-xs font-semibold uppercase text-gray-500 text-center">Customer Name</TableHead>
                    <TableHead className="px-3 py-2 text-xs font-semibold uppercase text-gray-500 text-center">Customer Email</TableHead>
                    <TableHead className="px-3 py-2 text-xs font-semibold uppercase text-gray-500 text-center">Details</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {filteredVehicles.map((row, idx) => (
                    <TableRow 
                      key={row.id || row.Id} 
                      className={`${idx % 2 === 0 ? "bg-white" : "bg-gray-50/60"} cursor-pointer hover:bg-gray-200 transition-colors`}
                      onClick={() => setSelectedVehicle(row)}
                    >
                      <TableCell className="px-3 py-2 font-mono whitespace-nowrap">{row.vin || row.VIN}</TableCell>
                      <TableCell className="px-3 py-2">{row.vehicleName || row.VehicleName}</TableCell>
                      <TableCell className="px-3 py-2">{(row.customer && (row.customer.name || row.customer.Name)) || ""}</TableCell>
                      <TableCell className="px-3 py-2 whitespace-nowrap">
                        <span className="inline-block max-w-[220px] truncate align-middle">{(row.customer && (row.customer.email || row.customer.Email)) || ""}</span>
                      </TableCell>
                      <TableCell className="px-3 py-2 text-center">
                        <div className="flex items-center justify-center gap-2">
                          <Badge variant="outline" className="text-xs">
                            {row.parts?.length || 0} Parts
                          </Badge>
                          <Badge variant="outline" className="text-xs">
                            {row.serviceHistories?.length || 0} Services
                          </Badge>
                          <Badge variant="outline" className="text-xs">
                            {row.warrantyClaims?.length || 0} Claims
                          </Badge>
                        </div>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </ScrollArea>
          )}
        </CardContent>
      </Card>

      {/* Vehicle Details Modal */}
      {selectedVehicle && (
        <Card className="shadow-sm border border-slate-200">
          <CardHeader>
            <div className="flex items-center justify-between">
              <CardTitle className="text-slate-800 flex items-center gap-2">
                <Car className="h-5 w-5" />
                Vehicle Details: {selectedVehicle.vehicleName || selectedVehicle.VehicleName}
              </CardTitle>
              <Button variant="outline" size="sm" onClick={() => setSelectedVehicle(null)}>
                Close
              </Button>
            </div>
          </CardHeader>
          <CardContent>
            <div className="grid gap-4 md:grid-cols-2 mb-6">
              <div className="flex items-center gap-4">
                <Label className="text-sm font-medium text-gray-500">VIN:</Label>
                <p className="font-mono text-sm">{selectedVehicle.vin || selectedVehicle.VIN}</p>
              </div>
              <div className="flex items-center gap-4">
                <Label className="text-sm font-medium text-gray-500">Customer:</Label>
                <p className="text-sm">
                  {selectedVehicle.customer?.name || selectedVehicle.customer?.Name || "N/A"}
                </p>
              </div>
              <div className="flex items-center gap-4">
                <Label className="text-sm font-medium text-gray-500">Email:</Label>
                <p className="text-sm">
                  {selectedVehicle.customer?.email || selectedVehicle.customer?.Email || "N/A"}
                </p>
              </div>
              <div className="flex items-center gap-4">
                <Label className="text-sm font-medium text-gray-500">Phone:</Label>
                <p className="text-sm">
                  {selectedVehicle.customer?.phone || selectedVehicle.customer?.Phone || "N/A"}
                </p>
              </div>
            </div>

            <Tabs defaultValue="parts" className="w-full">
              <TabsList className="grid w-full grid-cols-4">
                <TabsTrigger value="parts" className="flex items-center gap-2">
                  <Wrench className="h-4 w-4" />
                  Parts ({selectedVehicle.parts?.length || 0})
                </TabsTrigger>
                <TabsTrigger value="services" className="flex items-center gap-2">
                  <FileText className="h-4 w-4" />
                  Services ({selectedVehicle.serviceHistories?.length || 0})
                </TabsTrigger>
                <TabsTrigger value="claims" className="flex items-center gap-2">
                  <Shield className="h-4 w-4" />
                  Claims ({selectedVehicle.warrantyClaims?.length || 0})
                </TabsTrigger>
                <TabsTrigger value="summary" className="flex items-center gap-2">
                  <Car className="h-4 w-4" />
                  Summary
                </TabsTrigger>
              </TabsList>

              <TabsContent value="parts" className="mt-4">
                <ScrollArea className="h-[300px] rounded-md border">
                  {selectedVehicle.parts && selectedVehicle.parts.length > 0 ? (
                    <Table>
                      <TableHeader>
                        <TableRow>
                          <TableHead className="text-center">Name</TableHead>
                          <TableHead className="text-center">Serial Number</TableHead>
                        </TableRow>
                      </TableHeader>
                      <TableBody>
                        {selectedVehicle.parts.map((part, idx) => (
                          <TableRow key={part.id || idx}>
                            <TableCell>{part.name || part.Name}</TableCell>
                            <TableCell className="font-mono">{part.serialNumber || part.SerialNumber}</TableCell>
                          </TableRow>
                        ))}
                      </TableBody>
                    </Table>
                  ) : (
                    <div className="p-8 text-center text-gray-500">
                      <Wrench className="h-12 w-12 mx-auto mb-4 opacity-50" />
                      <p>No parts registered for this vehicle</p>
                    </div>
                  )}
                </ScrollArea>
              </TabsContent>

              <TabsContent value="services" className="mt-4">
                <ScrollArea className="h-[300px] rounded-md border">
                  {selectedVehicle.serviceHistories && selectedVehicle.serviceHistories.length > 0 ? (
                    <Table>
                      <TableHeader>
                        <TableRow>
                          <TableHead className="text-center">Date</TableHead>
                          <TableHead className="text-center">Description</TableHead>
                          <TableHead className="text-center">Technician</TableHead>
                        </TableRow>
                      </TableHeader>
                      <TableBody>
                        {selectedVehicle.serviceHistories.map((service, idx) => (
                          <TableRow key={service.id || idx}>
                            <TableCell className="whitespace-nowrap">
                              {new Date(service.serviceDate || service.ServiceDate).toLocaleDateString()}
                            </TableCell>
                            <TableCell>{service.description || service.Description}</TableCell>
                            <TableCell>{service.technician || service.Technician}</TableCell>
                          </TableRow>
                        ))}
                      </TableBody>
                    </Table>
                  ) : (
                    <div className="p-8 text-center text-gray-500">
                      <FileText className="h-12 w-12 mx-auto mb-4 opacity-50" />
                      <p>No service history recorded for this vehicle</p>
                    </div>
                  )}
                </ScrollArea>
              </TabsContent>

              <TabsContent value="claims" className="mt-4">
                <ScrollArea className="h-[300px] rounded-md border">
                  {selectedVehicle.warrantyClaims && selectedVehicle.warrantyClaims.length > 0 ? (
                    <Table>
                      <TableHeader>
                        <TableRow>
                          <TableHead className="text-center">Date</TableHead>
                          <TableHead className="text-center">Status</TableHead>
                          <TableHead className="text-center">Report</TableHead>
                          <TableHead className="text-center">Diagnostic</TableHead>
                        </TableRow>
                      </TableHeader>
                      <TableBody>
                        {selectedVehicle.warrantyClaims.map((claim, idx) => (
                          <TableRow key={claim.id || idx}>
                            <TableCell className="whitespace-nowrap">
                              {new Date(claim.claimDate || claim.ClaimDate).toLocaleDateString()}
                            </TableCell>
                            <TableCell>
                              <Badge className={getStatusBadgeColor(claim.status || claim.Status)}>
                                {claim.status || claim.Status}
                              </Badge>
                            </TableCell>
                            <TableCell className="max-w-[200px] truncate">
                              {claim.report || claim.Report}
                            </TableCell>
                            <TableCell className="max-w-[200px] truncate">
                              {claim.diagnosticInfo || claim.DiagnosticInfo}
                            </TableCell>
                          </TableRow>
                        ))}
                      </TableBody>
                    </Table>
                  ) : (
                    <div className="p-8 text-center text-gray-500">
                      <Shield className="h-12 w-12 mx-auto mb-4 opacity-50" />
                      <p>No warranty claims filed for this vehicle</p>
                    </div>
                  )}
                </ScrollArea>
              </TabsContent>

              <TabsContent value="summary" className="mt-4">
                <div className="grid gap-4 md:grid-cols-3">
                  <Card>
                    <CardContent className="p-4 text-center">
                      <Wrench className="h-8 w-8 mx-auto mb-2 text-gray-700" />
                      <p className="text-2xl font-bold">{selectedVehicle.parts?.length || 0}</p>
                      <p className="text-sm text-gray-500">Parts</p>
                    </CardContent>
                  </Card>
                  <Card>
                    <CardContent className="p-4 text-center">
                      <FileText className="h-8 w-8 mx-auto mb-2 text-gray-600" />
                      <p className="text-2xl font-bold">{selectedVehicle.serviceHistories?.length || 0}</p>
                      <p className="text-sm text-gray-500">Services</p>
                    </CardContent>
                  </Card>
                  <Card>
                    <CardContent className="p-4 text-center">
                      <Shield className="h-8 w-8 mx-auto mb-2 text-gray-500" />
                      <p className="text-2xl font-bold">{selectedVehicle.warrantyClaims?.length || 0}</p>
                      <p className="text-sm text-gray-500">Claims</p>
                    </CardContent>
                  </Card>
                </div>
              </TabsContent>
            </Tabs>
          </CardContent>
        </Card>
      )}
    </div>
  )
}
