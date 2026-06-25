import { formatDateOnly } from "@/lib/date"

const EXPENSES_URL = "/api/expenses"

export async function getExpenses(filters = {}) {
    const params = new URLSearchParams()

    if (filters.dateFrom) {
        params.append("dateFrom", formatDateOnly(filters.dateFrom))
    }

    if (filters.dateTo) {
        params.append("dateTo", formatDateOnly(filters.dateTo))
    }

    if (filters.categoryId) {
        params.append("categoryId", filters.categoryId)
    }

    if (filters.search) {
        params.append("search", filters.search)
    }

    const queryString = params.toString()
    const url = queryString ? `${EXPENSES_URL}?${queryString}` : EXPENSES_URL

    const response = await fetch(url)

    if (!response.ok) {
        throw new Error("Не удалось загрузить расходы")
    }

    return response.json()
}

export async function getExpenseById(id) {
    const response = await fetch(`${EXPENSES_URL}/${id}`)

    if (!response.ok) {
        throw new Error("Не удалось загрузить расход")
    }

    return response.json()
}

export async function createExpense(expense) {
    const response = await fetch(EXPENSES_URL, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(expense),
    })

    if (!response.ok) {
        throw new Error("Не удалось создать расход")
    }

    return response.json()
}

export async function updateExpense(id, expense) {
    const response = await fetch(`${EXPENSES_URL}/${id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(expense),
    })

    if (!response.ok) {
        throw new Error("Не удалось обновить расход")
    }
}

export async function deleteExpense(id) {
    const response = await fetch(`${EXPENSES_URL}/${id}`, {
        method: "DELETE",
    })

    if (!response.ok) {
        throw new Error("Не удалось удалить расход")
    }
}
