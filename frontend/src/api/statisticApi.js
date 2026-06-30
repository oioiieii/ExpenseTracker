import { formatDateOnly } from "@/lib/date"

const SUMMARY_URL = "/api/expenses/summary"

export async function getExpensesSummary(dateFrom, dateTo) {
    const params = new URLSearchParams()

    params.append("dateFrom", formatDateOnly(dateFrom))
    params.append("dateTo", formatDateOnly(dateTo))

    const response = await fetch(`${SUMMARY_URL}?${params}`)

    if (!response.ok) {
        throw new Error(await response.text())
    }

    return response.json()
}
