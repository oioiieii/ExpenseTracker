import { PencilIcon, TrashIcon } from "lucide-react"
import {
  ContextMenu,
  ContextMenuContent,
  ContextMenuItem,
  ContextMenuTrigger,
} from "@/components/ui/context-menu"
import { parseDateOnly } from "@/lib/date"

function ExpenseList({ expenses, categories, onEditExpense, onDeleteExpense }) {
  function getCategoryName(categoryId) {
    const category = categories.find((category) => category.id === categoryId)

    return category ? category.name : "Без категории"
  }

  if (expenses.length === 0) {
    return (
      <p className="text-sm text-muted-foreground text-center">
        Расходов пока нет
      </p>
    )
  }

  return (
    <div className="space-y-3">
      {expenses.map((expense) => (
        <div key={expense.id}>
          <ContextMenu>
            <ContextMenuTrigger className="block">
              <div className="rounded-lg border p-4">
                <div className="flex items-center justify-between gap-4">
                  <div>
                    <p className="font-medium">{expense.description}</p>
                    <p className="text-sm text-muted-foreground">
                      {getCategoryName(expense.categoryId)}
                    </p>
                  </div>

                  <div className="text-right">
                    <p className="font-semibold">{expense.amount} ₽</p>
                    <p className="text-sm text-muted-foreground">
                      {parseDateOnly(expense.date).toLocaleDateString("ru-RU")}
                    </p>
                  </div>
                </div>
              </div>
            </ContextMenuTrigger>
            <ContextMenuContent>
              <ContextMenuItem onSelect={() => onEditExpense(expense)}>
                <PencilIcon />
                Редактировать
              </ContextMenuItem>
              <ContextMenuItem
                variant="destructive"
                onSelect={() => onDeleteExpense(expense.id)}
              >
                <TrashIcon />
                Удалить
              </ContextMenuItem>
            </ContextMenuContent>
          </ContextMenu>
        </div>
      ))}
    </div>
  )
}

export default ExpenseList
