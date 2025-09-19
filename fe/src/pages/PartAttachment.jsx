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

// Validation schema with zod
const formSchema = z.object({
  vin: z.string().min(1, "Vehicle VIN is required"),
  partName: z.string().min(1, "Part name is required"),
  serial: z.string().min(1, "Serial number is required"),
})

export default function PartAttachment() {
  const [status, setStatus] = useState(null)
  const [loading, setLoading] = useState(false)

  const form = useForm({
    resolver: zodResolver(formSchema),
    defaultValues: { vin: "", partName: "", serial: "" },
  })

  const onSubmit = async (values) => {
    setStatus(null)
    setLoading(true)

    try {
      const res = await fetch("http://localhost:5165/api/parts", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          vehicleId: values.vin,
          name: values.partName,
          serialNumber: values.serial,
        }),
      })

      if (res.ok) {
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
    <Card className="max-w-lg mx-auto mt-6">
      <CardHeader>
        <CardTitle>Attach Part Serial to Vehicle</CardTitle>
      </CardHeader>
      <CardContent>
        <FormProvider {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-5">
            {["vin", "partName", "serial"].map((field) => (
              <FormField
                key={field}
                control={form.control}
                name={field}
                render={({ field: f }) => (
                  <FormItem>
                    <FormLabel>
                      {field === "vin"
                        ? "Vehicle VIN"
                        : field === "partName"
                        ? "Part Name"
                        : "Part Serial Number"}
                    </FormLabel>
                    <FormControl>
                      <Input placeholder={`Enter ${field}`} {...f} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            ))}

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
