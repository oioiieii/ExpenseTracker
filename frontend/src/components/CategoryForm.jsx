import { useState } from "react"
import { createCategory, updateCategory } from "@/api/categoriesApi"
import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"

function CategoryForm({ category, onCategorySaved, onCancelEdit }) {
  const [name, setName] = useState(category?.name ?? "")
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState(null)
  const isEditing = Boolean(category)

  async function handleSubmit(event) {
    event.preventDefault()

    try {
      setIsSubmitting(true)
      setError(null)

      const categoryData = {
        name: name.trim(),
      }

      if (isEditing) {
        await updateCategory(category.id, categoryData)
      } else {
        await createCategory(categoryData)
        setName("")
      }

      await onCategorySaved({ isEditing })
    } catch (error) {
      setError(error.message)
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>
          {isEditing ? "Редактировать категорию" : "Добавить категорию"}
        </CardTitle>
      </CardHeader>

      <CardContent>
        <form className="grid gap-4" onSubmit={handleSubmit}>
          <div className="grid gap-2">
            <Label htmlFor="category-name">Название</Label>
            <Input
              id="category-name"
              value={name}
              onChange={(event) => setName(event.target.value)}
              placeholder="Введите название"
              required
            />
          </div>

          <div className="flex gap-2">
            <Button className="flex-1" type="submit" disabled={isSubmitting}>
              {isSubmitting
                ? isEditing ? "Сохранение..." : "Добавление..."
                : isEditing ? "Сохранить" : "Добавить категорию"}
            </Button>

            {isEditing && (
              <Button type="button" variant="outline" onClick={onCancelEdit}>
                Отмена
              </Button>
            )}
          </div>

          {error && <p className="text-sm text-destructive">{error}</p>}
        </form>
      </CardContent>
    </Card>
  )
}

export default CategoryForm
