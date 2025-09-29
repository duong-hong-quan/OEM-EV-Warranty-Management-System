import React, { useState } from "react"
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Button } from "@/components/ui/button"
import { Alert, AlertDescription } from "@/components/ui/alert"
import { api, setAuthToken } from "@/lib/api"
import { setCurrentUser } from "@/lib/auth"
import { useNavigate } from "react-router-dom"

export default function Login() {
  const [email, setEmail] = useState("admin@warranty.com")
  const [password, setPassword] = useState("admin123")
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState("")
  const [me, setMe] = useState(null)
  const navigate = useNavigate()

  const handleLogin = async (e) => {
    e.preventDefault()
    setLoading(true)
    setError("")
    setMe(null)

    try {
      const res = await api.post("/api/auth/login", { Email: email, Password: password })
      const token = res.data?.token || res.data?.Token
      const user = res.data?.user || res.data?.User
      if (!token) throw new Error("No token in response")
      setAuthToken(token)
      setCurrentUser(user)
      setMe(user)
      navigate("/")
    } catch (err) {
      setError("Login failed. Please check your credentials.")
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="p-6 max-w-md mx-auto">
      <Card>
        <CardHeader>
          <CardTitle>Demo Login</CardTitle>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleLogin} className="space-y-4">
            <div>
              <Label htmlFor="email">Email</Label>
              <Input id="email" type="email" value={email} onChange={(e) => setEmail(e.target.value)} />
            </div>
            <div>
              <Label htmlFor="password">Password</Label>
              <Input id="password" type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
            </div>
            <Button type="submit" className="w-full" disabled={loading}>
              {loading ? "Signing in..." : "Sign in"}
            </Button>
          </form>

          {error && (
            <Alert variant="destructive" className="mt-4">
              <AlertDescription>{error}</AlertDescription>
            </Alert>
          )}

          {me && (
            <div className="mt-6 text-sm">
              <div><strong>User:</strong> {me.name || me.Name} ({me.email || me.Email})</div>
              <div><strong>Role:</strong> {me.role || me.Role}</div>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  )
} 