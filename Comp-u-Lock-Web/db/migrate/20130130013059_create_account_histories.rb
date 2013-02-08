class CreateAccountHistories < ActiveRecord::Migration
  def change
    create_table :account_histories do |t|
      t.references :account

      t.string :domain, :null => false, :default => ""
      t.string :url, :null => false, :default => ""
      t.string :title, :null => false, :default => ""
      t.datetime :last_visited, :null => false
      t.integer :visit_count, :null => false, :default => 0
      
      t.timestamps
    end
  end
end
