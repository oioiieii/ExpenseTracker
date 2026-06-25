import { format } from "date-fns"

export function formatDateOnly(date) {
  return format(date, "yyyy-MM-dd")
}

export function parseDateOnly(value) {
  if (value instanceof Date) {
    return value
  }

  const [year, month, day] = value.split("-").map(Number)

  return new Date(year, month - 1, day)
}
