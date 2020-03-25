import csv
import random

# movie, title, cost, genre, platform, releaseYear, director, duration
# 1784 movies

costRange = [0, 30]
genres = ["Action", "Romance", "Drama", "Comedy", "Fantasy", "Documentary"]
platforms = ["DVD", "VCR", "Blu-Ray"]
directors = ["Barack Obama", "Brudda Osas", "John Romero", "Wachowski Brothers", "John Mcafee", "Mitt Romney"]
durationRange = [80, 150]

with open('movies.csv') as csvfile:
    reader = csv.DictReader(csvfile)

    for row in reader:
        movie = []
        movie.append("Movie")
        movie.append((row["title"]).replace("&amp;", "&").replace("&#39;", "'").replace(",", " "))
        movie.append(random.randint(costRange[0], costRange[1]) + 0.99)
        movie.append(random.choice(genres))
        movie.append(random.choice(platforms))
        movie.append(row["year"])
        movie.append(random.choice(directors))
        movie.append(random.randint(durationRange[0], durationRange[1]))

        print(','.join([str(x) for x in movie]))


# book, title, cost, genre, platform, releaseYear, author, publisher
# 10,000 books

costRange = [0, 30]
genres = ["Action", "Romance", "Drama", "Comedy", "Fantasy", "Non-Fiction"]
platforms = ["Paperback", "Hardcover", "Kindle"]
authors = ["J.K Rowling", "Leo Tolstoy", "Ernest Hemingway", "William Shakespeare", "Mark Twain", "William Faulkner"]
publishers = ["Penguin", "Puffin", "Seagull", "Random", "Pearson", "Harper"]

with open('books.csv', mode="r", encoding="utf-8-sig") as csvfile:
    reader = csv.DictReader(csvfile)

    for row in reader:
        book = []
        book.append("Book")

        title = row["original_title"].replace(",", " ")
        if not title:
            continue
        
        book.append(title)
        
        book.append(random.randint(costRange[0], costRange[1]) + 0.99)
        book.append(random.choice(genres))
        book.append(random.choice(platforms))

        year = row["original_publication_year"][:-2]
        if not year or year[0] == "-":
            continue
        
        book.append(year)
        book.append(random.choice(authors))
        book.append(random.choice(publishers))

        print(','.join([str(x) for x in book]))
        
# game, title, cost, genre, platform, releaseYear, developer, rating

costRange = [30, 90]
platforms = ["Xbox One", "PS4", "Nintendo Switch", "PC"]
developers = ["Nintendo", "Bethesda", "Sony", "Ubisoft", "Rockstar"]
ratingRange = [1, 10]

with open('games.csv') as csvfile:
    reader = csv.DictReader(csvfile)

    for row in reader:
        game = []
        game.append("Game")
        game.append(row["Title"].replace(",", " "))
        game.append(random.randint(costRange[0], costRange[1]) + 0.99)
        game.append(row["Metadata.Genres"].replace(",", "/").replace(" / ", "/"))
        game.append(random.choice(platforms))
        game.append(row["Release.Year"])
        game.append(random.choice(developers))
        game.append(random.randint(ratingRange[0], ratingRange[1] + 1))

        print(','.join([str(x) for x in game]))
