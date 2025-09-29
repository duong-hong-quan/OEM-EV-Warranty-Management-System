import React, { useEffect, useState } from "react"
import { api } from "@/lib/api"
import {
  Card,
  CardHeader,
  CardTitle,
  CardContent,
} from "@/components/ui/card"
import { Alert, AlertDescription } from "@/components/ui/alert"
import { ScrollArea } from "@/components/ui/scroll-area"
import { Loader2, RefreshCcw } from "lucide-react"
import {
  Table,
  TableHeader,
  TableRow,
  TableHead,
  TableBody,
  TableCell,
} from "@/components/ui/table"
import { Button } from "@/components/ui/button"

export default function ServiceHistory() {
  const [history, setHistory] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)

  const fetchHistory = async () => {
    setLoading(true)
    setError(null)
    try {
      const res = await api.get("/api/service-history")
      setHistory(res.data?.items || [])
    } catch {
      setError("Failed to load service history.")
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    fetchHistory()
  }, [])

  return (
    <div className="p-6">
      <Card className="shadow-sm border border-slate-200">
        <CardHeader className="flex flex-row items-center justify-between">
          <CardTitle className="text-slate-800">Service & Warranty History</CardTitle>
          <Button
            variant="outline"
            size="sm"
            onClick={fetchHistory}
            disabled={loading}
            className="gap-2"
          >
            <RefreshCcw className={loading ? "h-4 w-4 animate-spin" : "h-4 w-4"} />
            Refresh
          </Button>
        </CardHeader>

        <CardContent>
          {loading ? (
            <div className="flex items-center justify-center py-10 text-slate-500">
              <Loader2 className="mr-2 h-5 w-5 animate-spin" />
              Loading service history...
            </div>
          ) : error ? (
            <Alert variant="destructive">
              <AlertDescription>{error}</AlertDescription>
            </Alert>
          ) : history.length === 0 ? (
            <Alert>
              <AlertDescription>No service history available.</AlertDescription>
            </Alert>
          ) : (
            <ScrollArea className="h-[400px] rounded-md border">
              <Table>
                <TableHeader className="bg-slate-50">
                  <TableRow>
                    <TableHead className="px-3 py-2 text-xs font-semibold uppercase text-slate-500">Date</TableHead>
                    <TableHead className="px-3 py-2 text-xs font-semibold uppercase text-slate-500">Description</TableHead>
                    <TableHead className="px-3 py-2 text-xs font-semibold uppercase text-slate-500">Technician</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {history.map((h, idx) => (
                    <TableRow
                      key={h.id}
                      className={idx % 2 === 0 ? "bg-white" : "bg-slate-50/60"}
                    >
                      <TableCell className="px-3 py-2 whitespace-nowrap">
                        {new Date(h.serviceDate).toLocaleDateString("en-US", {
                          year: "numeric",
                          month: "short",
                          day: "numeric",
                        })}
                      </TableCell>
                      <TableCell className="px-3 py-2">{h.description}</TableCell>
                      <TableCell className="px-3 py-2">{h.technician}</TableCell>
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
