import { useState } from "react"
import { format } from "date-fns"
import { ru } from "date-fns/locale"
import { Calendar as CalendarIcon } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Calendar } from "@/components/ui/calendar"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover"
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"

const DEFAULT_FILTERS = {
  dateFrom: undefined,
  dateTo: undefined,
  categoryId: "",
  search: "",
}

function getDateLabel(date, placeholder) {
  if (date) {
    return format(date, "PPP", { locale: ru })
  }

  return placeholder
}

function ExpenseFilters({ categories, onApplyFilters }) {
  const [filters, setFilters] = useState(DEFAULT_FILTERS)

  function updateFilter(name, value) {
    setFilters((currentFilters) => ({
      ...currentFilters,
      [name]: value,
    }))
  }

  function handleSubmit(event) {
    event.preventDefault()

    onApplyFilters({
      ...filters,
      categoryId: filters.categoryId === "all" ? "" : filters.categoryId,
      search: filters.search.trim(),
    })
  }

  function handleReset() {
    setFilters(DEFAULT_FILTERS)
    onApplyFilters(DEFAULT_FILTERS)
  }

  return (
    <Card className="mb-6" >
      <CardHeader>
        <CardTitle>Фильтры</CardTitle>
      </CardHeader>

      <CardContent>
        <form className="grid gap-4 md:grid-cols-4" onSubmit={handleSubmit}>
          <div className="grid gap-2">
            <Label>Дата с</Label>
            <Popover>
              <PopoverTrigger asChild>
                <Button variant="outline" data-empty={!filters.dateFrom}>
                  <CalendarIcon />
                  {getDateLabel(filters.dateFrom, "Выберите дату")}
                </Button>
              </PopoverTrigger>
              <PopoverContent className="w-auto p-0">
                <Calendar
                  mode="single"
                  selected={filters.dateFrom}
                  onSelect={(date) => updateFilter("dateFrom", date)}
                />
              </PopoverContent>
            </Popover>
          </div>

          <div className="grid gap-2">
            <Label>Дата по</Label>
            <Popover>
              <PopoverTrigger asChild>
                <Button variant="outline" data-empty={!filters.dateTo}>
                  <CalendarIcon />
                  {getDateLabel(filters.dateTo, "Выберите дату")}
                </Button>
              </PopoverTrigger>
              <PopoverContent className="w-auto p-0">
                <Calendar
                  mode="single"
                  selected={filters.dateTo}
                  onSelect={(date) => updateFilter("dateTo", date)}
                />
              </PopoverContent>
            </Popover>
          </div>

          <div className="grid gap-2">
            <Label>Категория</Label>
            <Select
              value={filters.categoryId || "all"}
              onValueChange={(value) => updateFilter("categoryId", value)}
            >
              <SelectTrigger className="w-full">
                <SelectValue placeholder="Все категории" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="all">Все категории</SelectItem>
                {categories.map((category) => (
                  <SelectItem key={category.id} value={category.id}>
                    {category.name}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>

          <div className="grid gap-2">
            <Label htmlFor="filter-search">Поиск</Label>
            <Input
              id="filter-search"
              value={filters.search}
              onChange={(event) => updateFilter("search", event.target.value)}
              placeholder="Описание"
            />
          </div>

          <div className="flex gap-2 md:col-span-4">
            <Button type="submit">Применить</Button>
            <Button type="button" variant="outline" onClick={handleReset}>
              Сбросить
            </Button>
          </div>
        </form>
      </CardContent>
    </Card>
  )
}

export default ExpenseFilters
