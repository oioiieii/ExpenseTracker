import {useEffect, useMemo, useState} from "react"
import {format} from "date-fns"
import {ru} from "date-fns/locale"
import {Calendar as CalendarIcon} from "lucide-react"
import {Bar, BarChart, CartesianGrid, XAxis, YAxis} from "recharts"
import {getExpensesSummary} from "@/api/statisticApi"
import {Button} from "@/components/ui/button"
import {Calendar} from "@/components/ui/calendar"
import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"
import {
    ChartContainer,
    ChartTooltip,
    ChartTooltipContent,
} from "@/components/ui/chart"
import {Label} from "@/components/ui/label"
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
import {parseDateOnly} from "@/lib/date"
import {ScrollArea, ScrollBar} from "@/components/ui/scroll-area"

const chartConfig = {
    amount: {
        label: "Расходы",
        color: "var(--primary)",
    },
}

function getDefaultDateFrom() {
    const now = new Date()
    return new Date(now.getFullYear(), now.getMonth(), 1)
}

function getDefaultDateTo() {
    const now = new Date()
    return new Date(now.getFullYear(), now.getMonth(), now.getDate())
}

function formatCurrency(value) {
    return new Intl.NumberFormat("ru-RU", {
        style: "currency",
        currency: "RUB",
        maximumFractionDigits: 0,
    }).format(value)
}

function getPeriodPickerLabel(dateFrom, dateTo) {
    if (dateFrom && dateTo) {
        return `${format(dateFrom, "PPP", {locale: ru})} - ${format(dateTo, "PPP", {locale: ru})}`
    }

    if (dateFrom) {
        return format(dateFrom, "PPP", {locale: ru})
    }

    return "Выберите период"
}

function ExpenseStats({refreshKey}) {
    const [summary, setSummary] = useState(null)
    const [dateFrom, setDateFrom] = useState(getDefaultDateFrom)
    const [dateTo, setDateTo] = useState(getDefaultDateTo)
    const [groupBy, setGroupBy] = useState("day")
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState(null)

    const period = useMemo(() => {
        if (dateFrom <= dateTo) {
            return {dateFrom, dateTo}
        }

        return {
            dateFrom: dateTo,
            dateTo: dateFrom,
        }
    }, [dateFrom, dateTo])

    function handlePeriodSelect(period) {
        if (!period?.from) {
            return
        }

        setDateFrom(period.from)
        setDateTo(period.to ?? period.from)
    }

    useEffect(() => {
        async function loadSummary() {
            try {
                setIsLoading(true)
                setError(null)

                const summaryData = await getExpensesSummary(period.dateFrom, period.dateTo)
                setSummary(summaryData)
            } catch (error) {
                setError(error.message)
            } finally {
                setIsLoading(false)
            }
        }

        loadSummary()
    }, [period, refreshKey])

    const groupedExpenses = useMemo(() => {
        if (!summary) {
            return []
        }

        if (groupBy === "category") {
            return summary.totalExpensesGroupByCategory.map((item) => ({
                id: item.categoryId ?? "without-category",
                label: item.categoryName || "Без категории",
                amount: item.amount,
            }))
        }

        return summary.totalExpensesGroupByDay.map((item) => ({
            id: item.date,
            label: format(parseDateOnly(item.date), "d MMM", {locale: ru}),
            amount: item.amount,
        }))
    }, [groupBy, summary])

    return (
        <Card>
            <CardHeader>
                <CardTitle>Статистика</CardTitle>
            </CardHeader>
            <CardContent>
                {isLoading && <p>Загрузка статистики...</p>}

                {error && <p className="text-sm text-destructive">{error}</p>}

                {!isLoading && !error && summary && (
                    <div className="grid gap-6 lg:grid-cols-[1fr_320px]">
                        <div className="space-y-4">
                            <ChartContainer config={chartConfig}>
                                <BarChart data={groupedExpenses}>
                                    <CartesianGrid vertical={false}/>
                                    <XAxis
                                        dataKey="label"
                                        tickLine={false}
                                        tickMargin={10}
                                        axisLine={false}
                                    />
                                    <YAxis
                                        tickLine={false}
                                        axisLine={false}
                                        tickFormatter={(value) => `${value}`}
                                    />
                                    <ChartTooltip
                                        cursor={false}
                                        content={<ChartTooltipContent/>}
                                    />
                                    <Bar
                                        dataKey="amount"
                                        fill="var(--color-amount)"
                                        maxBarSize={48}
                                        radius={6}
                                    />
                                </BarChart>
                            </ChartContainer>
                        </div>

                        <div>
                            <div className="mb-6 grid gap-4">
                                <div className="grid gap-2">
                                    <Label>Период</Label>
                                    <Popover>
                                        <PopoverTrigger asChild>
                                            <Button variant="outline">
                                                <CalendarIcon/>
                                                {getPeriodPickerLabel(dateFrom, dateTo)}
                                            </Button>
                                        </PopoverTrigger>
                                        <PopoverContent className="w-auto p-0">
                                            <Calendar
                                                mode="range"
                                                selected={{
                                                    from: dateFrom,
                                                    to: dateTo,
                                                }}
                                                onSelect={handlePeriodSelect}
                                                numberOfMonths={2}
                                            />
                                        </PopoverContent>
                                    </Popover>
                                </div>
                                <div className="grid gap-2">
                                    <Label>Группировка</Label>
                                    <Select value={groupBy} onValueChange={setGroupBy}>
                                        <SelectTrigger className="w-full">
                                            <SelectValue/>
                                        </SelectTrigger>
                                        <SelectContent>
                                            <SelectItem value="day">По дням</SelectItem>
                                            <SelectItem value="category">По категориям</SelectItem>
                                        </SelectContent>
                                    </Select>
                                </div>
                            </div>

                            <p className="mb-3 text-sm font-medium">Расходы за выбранный период</p>
                            <div className="my-6">
                                <p className="text-sm text-muted-foreground text-center">Всего расходов</p>
                                <p className="text-3xl font-semibold text-center">
                                    {formatCurrency(summary.totalExpenses)}
                                </p>
                            </div>
                            <ScrollArea>
                                <div className={"space-y-3"}>
                                    {groupedExpenses.length === 0 && (
                                        <p className="text-sm text-muted-foreground">Нет расходов за период</p>
                                    )}

                                    {groupedExpenses.map((expenseGroup) => (
                                        <div
                                            key={expenseGroup.id}
                                            className="flex items-center justify-between rounded-lg border p-3"
                                        >
                    <span className="text-sm">
                      {expenseGroup.label}
                    </span>
                                            <span className="font-medium">
                      {formatCurrency(expenseGroup.amount)}
                    </span>
                                        </div>
                                    ))}
                                </div>
                            </ScrollArea>
                        </div>
                    </div>
                )}
            </CardContent>
        </Card>
    )
}

export default ExpenseStats
