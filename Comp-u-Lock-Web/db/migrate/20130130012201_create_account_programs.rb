class CreateAccountPrograms < ActiveRecord::Migration
  def change
    create_table :account_programs do |t|
      t.references :account

      t.string :name, :null => false, :default => ""
      t.datetime :lastrun
      t.integer :open_count, :null => false, :default => 0

      t.timestamps
    end
  end
end
