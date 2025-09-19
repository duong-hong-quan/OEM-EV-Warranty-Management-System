import React, { useEffect, useState } from "react"
import axios from "axios"
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
      const res = await axios.get("http://localhost:5165/api/servicehistory")
      setHistory(res.data)
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
      <Card className="shadow-sm">
        <CardHeader className="flex flex-row items-center justify-between">
          <CardTitle>Service & Warranty History</CardTitle>
          <Button
            variant="outline"
            size="sm"
            onClick={fetchHistory}
            disabled={loading}
          >
            <RefreshCcw
              className={`h-4 w-4 mr-2 ${
                loading ? "animate-spin" : ""
              }`}
            />
            Refresh
          </Button>
        </CardHeader>

        <CardContent>
          {loading ? (
            <div className="flex items-center justify-center py-10 text-gray-500">
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
                <TableHeader>
                  <TableRow>
                    <TableHead>Date</TableHead>
                    <TableHead>Description</TableHead>
                    <TableHead>Technician</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {history.map((h, idx) => (
                    <TableRow
                      key={h.id}
                      className={idx % 2 === 0 ? "bg-gray-50/50" : ""}
                    >
                      <TableCell className="whitespace-nowrap">
                        {new Date(h.serviceDate).toLocaleDateString("en-US", {
                          year: "numeric",
                          month: "short",
                          day: "numeric",
                        })}
                      </TableCell>
                      <TableCell>{h.description}</TableCell>
                      <TableCell>{h.technician}</TableCell>
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
