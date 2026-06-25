const CATEGORIES_URL = "/api/categories"

export async function getCategories() {
    const response = await fetch(CATEGORIES_URL)

    if (!response.ok) {
        throw new Error("Не удалось загрузить категории")
    }

    return response.json()
}

export async function getCategoryById(id) {
    const response = await fetch(`${CATEGORIES_URL}/${id}`)

    if (!response.ok) {
        throw new Error("Не удалось загрузить категорию")
    }

    return response.json()
}

export async function createCategory(category) {
    const response = await fetch(CATEGORIES_URL, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(category),
    })

    if (!response.ok) {
        throw new Error("Не удалось создать категорию")
    }

    return response.json()
}

export async function updateCategory(id, category) {
    const response = await fetch(`${CATEGORIES_URL}/${id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(category),
    })

    if (!response.ok) {
        throw new Error("Не удалось обновить категорию")
    }
}

export async function deleteCategory(id) {
    const response = await fetch(`${CATEGORIES_URL}/${id}`, {
        method: "DELETE",
    })

    if (!response.ok) {
        throw new Error("Не удалось удалить категорию")
    }
}
