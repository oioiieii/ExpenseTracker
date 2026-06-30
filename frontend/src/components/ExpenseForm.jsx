import {useState} from "react"
import {createExpense, updateExpense} from "@/api/expensesApi"
import {Button} from "@/components/ui/button"
import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"
import {Input} from "@/components/ui/input"
import {Label} from "@/components/ui/label"
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select"
import {format} from "date-fns"
import {ru} from "date-fns/locale"
import {Calendar as CalendarIcon} from "lucide-react"
import {Calendar} from "@/components/ui/calendar"
import {formatDateOnly, parseDateOnly} from "@/lib/date"
import {
    Popover,
    PopoverContent,
    PopoverTrigger,
} from "@/components/ui/popover"

function ExpenseForm({categories, expense, onExpenseSaved, onCancelEdit}) {
    const [description, setDescription] = useState(expense?.description ?? "")
    const [amount, setAmount] = useState(expense ? String(expense.amount) : "")
    const [date, setDate] = useState(expense ? parseDateOnly(expense.date) : new Date())
    const [categoryId, setCategoryId] = useState(expense?.categoryId ?? "")
    const [isSubmitting, setIsSubmitting] = useState(false)
    const [error, setError] = useState(null)
    const isEditing = Boolean(expense)

    async function handleSubmit(event) {
        event.preventDefault()

        try {
            setIsSubmitting(true)
            setError(null)

            if (!categoryId) {
                setError("Выберите категорию.");
                return;
            }

            const expenseData = {
                description: description.trim(),
                amount: Number(amount),
                categoryId: categoryId,
                date: formatDateOnly(date),
            }

            if (isEditing) {
                await updateExpense(expense.id, expenseData)
            } else {
                await createExpense(expenseData)
                setDescription("")
                setAmount("")
                setDate(new Date())
                setCategoryId("")
            }

            await onExpenseSaved()
        } catch (error) {
            setError(error.message)
        } finally {
            setIsSubmitting(false)
        }
    }

    return (
        <Card className="mb-6">
            <CardHeader>
                <CardTitle>
                    {isEditing ? "Редактировать расход" : "Добавить расход"}
                </CardTitle>
            </CardHeader>

            <CardContent>
                <form onSubmit={handleSubmit} className="flex flex-col gap-3">
                    <Label htmlFor="expense-description">Описание</Label>
                    <Input
                        id="expense-description"
                        value={description}
                        onChange={(event) => setDescription(event.target.value)}
                        placeholder="Введите описание"
                        required
                    />

                    <Label htmlFor="expense-amount">Сумма</Label>
                    <Input
                        id="expense-amount"
                        type="number"
                        min="1"
                        step="0.01"
                        value={amount}
                        onChange={(event) => setAmount(event.target.value)}
                        placeholder="0"
                        required
                    />

                    <Label>Дата</Label>
                    <Popover>
                        <PopoverTrigger asChild>
                            <Button
                                variant="outline"
                                data-empty={!date}
                            >
                                <CalendarIcon/>
                                {date ? format(date, "PPP", {locale: ru}) : <span>Выберите дату</span>}
                            </Button>
                        </PopoverTrigger>
                        <PopoverContent className="w-auto p-0">
                            <Calendar
                                mode="single"
                                selected={date}
                                onSelect={(selectedDate) => selectedDate && setDate(selectedDate)}
                            />
                        </PopoverContent>
                    </Popover>

                    <Label>Категория</Label>
                    <Select value={categoryId} onValueChange={setCategoryId}>
                        <SelectTrigger className="w-full">
                            <SelectValue placeholder="Выберите категорию"/>
                        </SelectTrigger>
                        <SelectContent>
                            {categories.map((category) => (
                                <SelectItem key={category.id} value={category.id}>
                                    {category.name}
                                </SelectItem>
                            ))}
                        </SelectContent>
                    </Select>

                    <div className="flex gap-2">
                        <Button className="flex-1" type="submit" disabled={isSubmitting}>
                            {isSubmitting
                                ? isEditing ? "Сохранение..." : "Добавление..."
                                : isEditing ? "Сохранить" : "Добавить"}
                        </Button>

                        {isEditing && (
                            <Button type="button" variant="outline" onClick={onCancelEdit}>
                                Отмена
                            </Button>
                        )}
                    </div>

                    {error && (
                        <p className="text-sm text-destructive md:col-span-4">{error}</p>
                    )}
                </form>
            </CardContent>
        </Card>
    )
}

export default ExpenseForm
