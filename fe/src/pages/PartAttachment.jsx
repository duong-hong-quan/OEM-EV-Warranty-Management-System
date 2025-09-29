import React, { useState } from "react"
import { useForm, FormProvider } from "react-hook-form"
import { z } from "zod"
import { zodResolver } from "@hookform/resolvers/zod"

import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Loader2 } from "lucide-react"
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import {
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
  FormControl,
} from "@/components/ui/form"
import { Alert, AlertDescription } from "@/components/ui/alert"
import { api } from "@/lib/api"
import VehicleSelect from "@/components/VehicleSelect"

// Validation schema with zod
const formSchema = z.object({
  vehicleId: z.string().uuid({ message: "Vehicle is required" }),
  partName: z.string().min(1, "Part name is required"),
  serial: z.string().min(1, "Serial number is required"),
})

export default function PartAttachment() {
  const [status, setStatus] = useState(null)
  const [loading, setLoading] = useState(false)

  const form = useForm({
    resolver: zodResolver(formSchema),
    defaultValues: { vehicleId: "", partName: "", serial: "" },
  })

  const onSubmit = async (values) => {
    setStatus(null)
    setLoading(true)

    try {
      const res = await api.post("/api/parts", {
        VehicleId: values.vehicleId,
        Name: values.partName,
        SerialNumber: values.serial,
      })

      if (res.status === 201 || res.status === 200) {
        setStatus({ type: "success", message: "✅ Part attached successfully!" })
        form.reset()
      } else {
        setStatus({ type: "error", message: "❌ Failed to attach part." })
      }
    } catch {
      setStatus({ type: "error", message: "⚠️ Error connecting to server." })
    } finally {
      setLoading(false)
    }
  }

  return (
    <Card className="max-w-2xl mx-auto mt-6 shadow-sm border border-slate-200">
      <CardHeader>
        <CardTitle className="text-slate-800">Attach Part Serial to Vehicle</CardTitle>
      </CardHeader>
      <CardContent>
        <FormProvider {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-5">
            <FormField
              control={form.control}
              name="vehicleId"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Vehicle</FormLabel>
                  <FormControl>
                    <VehicleSelect value={field.value} onChange={field.onChange} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="partName"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Part Name</FormLabel>
                  <FormControl>
                    <Input placeholder="Enter part name" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="serial"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Part Serial Number</FormLabel>
                  <FormControl>
                    <Input placeholder="Enter serial" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <Button type="submit" className="w-full" disabled={loading}>
              {loading && <Loader2 className="animate-spin h-4 w-4 mr-2" />}
              {loading ? "Attaching..." : "Attach Part"}
            </Button>
          </form>
        </FormProvider>

        {status && (
          <Alert
            className={`mt-4 ${
              status.type === "success" ? "border-green-500 text-green-700" : "border-red-500 text-red-700"
            }`}
          >
            <AlertDescription>{status.message}</AlertDescription>
          </Alert>
        )}
      </CardContent>
    </Card>
  )
}
