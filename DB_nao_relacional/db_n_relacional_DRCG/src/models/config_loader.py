"""Leitura dos arquivos de configuração (.ini)."""

from __future__ import annotations

from configparser import ConfigParser
from pathlib import Path


class ConfigLoader:
    def __init__(self, project_root: Path) -> None:
        self.project_root = project_root

    def load_sqlite_config(self) -> dict:
        parser = ConfigParser()
        parser.read(self.project_root / "config" / "sqlite.ini", encoding="utf-8")
        relative_path = parser.get("sqlite", "db_path")
        full_path = self.project_root / relative_path
        return {
            "db_path": str(full_path),
            "source_file": str(self.project_root / "config" / "sqlite.ini"),
        }

    def load_mongodb_config(self) -> dict:
        parser = ConfigParser()
        parser.read(self.project_root / "config" / "mongodb.ini", encoding="utf-8")
        return {
            "uri": parser.get("mongodb", "uri"),
            "database": parser.get("mongodb", "database"),
            "use_mongomock_on_failure": parser.getboolean("mongodb", "use_mongomock_on_failure"),
            "source_file": str(self.project_root / "config" / "mongodb.ini"),
        }

    def load_cassandra_config(self) -> dict:
        parser = ConfigParser()
        parser.read(self.project_root / "config" / "cassandra.ini", encoding="utf-8")
        hosts_raw = parser.get("cassandra", "hosts", fallback="localhost")
        return {
            "hosts": [h.strip() for h in hosts_raw.split(",")],
            "port": parser.getint("cassandra", "port", fallback=9042),
            "keyspace": parser.get("cassandra", "keyspace", fallback="ecommerce"),
            "use_fake_on_failure": parser.getboolean(
                "cassandra", "use_fake_on_failure", fallback=True
            ),
            "source_file": str(self.project_root / "config" / "cassandra.ini"),
        }

    def load_neo4j_config(self) -> dict:
        parser = ConfigParser()
        parser.read(self.project_root / "config" / "neo4j.ini", encoding="utf-8")
        return {
            "uri": parser.get("neo4j", "uri", fallback="bolt://localhost:7687"),
            "user": parser.get("neo4j", "user", fallback="neo4j"),
            "password": parser.get("neo4j", "password", fallback="neo4j_password"),
            "database": parser.get("neo4j", "database", fallback="neo4j"),
            "use_fake_on_failure": parser.getboolean(
                "neo4j", "use_fake_on_failure", fallback=True
            ),
            "source_file": str(self.project_root / "config" / "neo4j.ini"),
        }
