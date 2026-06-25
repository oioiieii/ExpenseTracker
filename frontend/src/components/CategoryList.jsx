import { PencilIcon, TrashIcon } from "lucide-react"
import {
  ContextMenu,
  ContextMenuContent,
  ContextMenuItem,
  ContextMenuTrigger,
} from "@/components/ui/context-menu"

function CategoryList({ categories, onEditCategory, onDeleteCategory }) {
  if (categories.length === 0) {
    return (
      <p className="text-sm text-muted-foreground text-center">
        Категорий пока нет
      </p>
    )
  }

  return (
    <div className="space-y-3">
      {categories.map((category) => (
        <div key={category.id}>
          <ContextMenu>
            <ContextMenuTrigger className="block">
              <div className="rounded-lg border p-4">
                <p className="font-medium">{category.name}</p>
              </div>
            </ContextMenuTrigger>
            <ContextMenuContent>
              <ContextMenuItem onSelect={() => onEditCategory(category)}>
                <PencilIcon />
                Редактировать
              </ContextMenuItem>
              <ContextMenuItem
                variant="destructive"
                onSelect={() => onDeleteCategory(category.id)}
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

export default CategoryList
